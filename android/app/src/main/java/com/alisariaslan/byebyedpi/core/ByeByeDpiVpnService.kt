package com.alisariaslan.byebyedpi.core

import android.app.Notification
import android.app.NotificationChannel
import android.app.NotificationManager
import android.app.PendingIntent
import android.content.Context
import android.content.Intent
import android.net.VpnService
import android.os.Build
import android.os.ParcelFileDescriptor
import androidx.core.app.NotificationCompat
import com.alisariaslan.byebyedpi.MainActivity
import com.alisariaslan.byebyedpi.R
import com.alisariaslan.byebyedpi.data.*
import kotlinx.coroutines.*
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.first

class ByeByeDpiVpnService : VpnService() {

    companion object {
        const val NOTIFICATION_CHANNEL_ID = "byebyedpi_vpn_channel"
        const val NOTIFICATION_ID = 1001

        const val ACTION_START = "com.alisariaslan.byebyedpi.START"
        const val ACTION_STOP = "com.alisariaslan.byebyedpi.STOP"

        private val _vpnStatus = MutableStateFlow(VpnStatus())
        val vpnStatus: StateFlow<VpnStatus> = _vpnStatus.asStateFlow()

        fun start(context: Context) {
            val intent = Intent(context, ByeByeDpiVpnService::class.java).apply {
                action = ACTION_START
            }
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
                context.startForegroundService(intent)
            } else {
                context.startService(intent)
            }
        }

        fun stop(context: Context) {
            val intent = Intent(context, ByeByeDpiVpnService::class.java).apply {
                action = ACTION_STOP
            }
            context.startService(intent)
        }
    }

    private var tunInterface: ParcelFileDescriptor? = null
    private var engineController: DpiEngineController? = null
    private var serviceScope = CoroutineScope(Dispatchers.IO + SupervisorJob())
    private var timerJob: Job? = null
    private var startTimeMs: Long = 0

    override fun onCreate() {
        super.onCreate()
        engineController = DpiEngineController(this)
        createNotificationChannel()
    }

    override fun onStartCommand(intent: Intent?, flags: Int, startId: Int): Int {
        when (intent?.action) {
            ACTION_START -> {
                startVpnTunnel()
                return START_STICKY
            }
            ACTION_STOP -> {
                stopVpnTunnel()
                stopSelf()
                return START_NOT_STICKY
            }
            else -> return START_NOT_STICKY
        }
    }

    private fun startVpnTunnel() {
        _vpnStatus.value = _vpnStatus.value.copy(state = VpnState.CONNECTING)
        DpiEngineController.log("[VPN] Tünel başlatılıyor...")

        serviceScope.launch {
            try {
                val settingsRepo = SettingsRepository(applicationContext)
                val presetId = settingsRepo.selectedPresetId.first()
                val preset = PresetRepository.getById(presetId)
                val customArgs = settingsRepo.customArguments.first()
                val effectiveArgs = if (preset.id == "custom") customArgs else preset.arguments

                val dnsProvider = settingsRepo.dnsProvider.first()
                val customPrimary = settingsRepo.customDnsPrimary.first()
                val customSecondary = settingsRepo.customDnsSecondary.first()

                val splitTunnelEnabled = settingsRepo.splitTunnelEnabled.first()
                val splitMode = settingsRepo.splitTunnelMode.first()
                val selectedPackages = settingsRepo.selectedPackages.first()

                // Configure VPN interface
                val builder = Builder()
                    .setSession("ByeByeDPI")
                    .setMtu(1500)
                    .addAddress("10.0.0.2", 32)
                    .addRoute("0.0.0.0", 0)

                // DNS Configuration
                when (dnsProvider) {
                    DnsProvider.CUSTOM -> {
                        if (customPrimary.isNotBlank()) builder.addDnsServer(customPrimary)
                        if (customSecondary.isNotBlank()) builder.addDnsServer(customSecondary)
                    }
                    else -> {
                        builder.addDnsServer(dnsProvider.primary)
                        builder.addDnsServer(dnsProvider.secondary)
                    }
                }

                // Split Tunneling
                if (splitTunnelEnabled && selectedPackages.isNotEmpty()) {
                    selectedPackages.forEach { pkg ->
                        try {
                            if (splitMode == SplitTunnelMode.ALLOW_SELECTED) {
                                builder.addAllowedApplication(pkg)
                            } else {
                                builder.addDisallowedApplication(pkg)
                            }
                        } catch (e: Exception) {
                            DpiEngineController.log("[WARN] Paket eklenemedi: $pkg - ${e.message}")
                        }
                    }
                }

                // Establish TUN
                tunInterface = builder.establish()

                if (tunInterface == null) {
                    throw IllegalStateException("VPN arayüzü oluşturulamadı (tunInterface == null). İzinleri kontrol edin.")
                }

                // Start DPI Engine
                val engineStarted = engineController?.start(
                    arguments = effectiveArgs,
                    protectCallback = { socketFd -> protect(socketFd) }
                ) ?: false

                if (!engineStarted) {
                    throw IllegalStateException("DPI Motoru başlatılamadı.")
                }

                startTimeMs = System.currentTimeMillis()
                _vpnStatus.value = VpnStatus(
                    state = VpnState.CONNECTED,
                    activePresetName = preset.name,
                    activeArguments = effectiveArgs,
                    uptimeSeconds = 0
                )

                startForeground(NOTIFICATION_ID, buildNotification(preset.name, 0))
                startUptimeCounter(preset.name)
                DpiEngineController.log("[VPN] ByeByeDPI tüneli başarıyla bağlandı!")

            } catch (e: Exception) {
                DpiEngineController.log("[ERROR] Tünel hatası: ${e.message}")
                _vpnStatus.value = VpnStatus(
                    state = VpnState.ERROR,
                    errorMessage = e.message ?: "Bilinmeyen hata"
                )
                stopVpnTunnel()
                stopSelf()
            }
        }
    }

    private fun startUptimeCounter(presetName: String) {
        timerJob?.cancel()
        timerJob = serviceScope.launch {
            while (isActive) {
                delay(1000)
                val uptime = (System.currentTimeMillis() - startTimeMs) / 1000
                _vpnStatus.value = _vpnStatus.value.copy(uptimeSeconds = uptime)

                // Update notification every 30 seconds
                if (uptime % 30 == 0L) {
                    val nm = getSystemService(Context.NOTIFICATION_SERVICE) as NotificationManager
                    nm.notify(NOTIFICATION_ID, buildNotification(presetName, uptime))
                }
            }
        }
    }

    private fun stopVpnTunnel() {
        _vpnStatus.value = _vpnStatus.value.copy(state = VpnState.DISCONNECTING)
        timerJob?.cancel()
        timerJob = null

        try {
            engineController?.stop()
        } catch (_: Exception) {}

        try {
            tunInterface?.close()
        } catch (_: Exception) {}
        tunInterface = null

        _vpnStatus.value = VpnStatus(state = VpnState.DISCONNECTED)
        DpiEngineController.log("[VPN] ByeByeDPI bağlantısı sonlandırıldı.")
        stopForeground(STOP_FOREGROUND_REMOVE)
    }

    private fun buildNotification(presetName: String, uptimeSeconds: Long): Notification {
        val openAppIntent = Intent(this, MainActivity::class.java).apply {
            flags = Intent.FLAG_ACTIVITY_SINGLE_TOP or Intent.FLAG_ACTIVITY_CLEAR_TOP
        }
        val openAppPendingIntent = PendingIntent.getActivity(
            this, 0, openAppIntent,
            PendingIntent.FLAG_UPDATE_CURRENT or PendingIntent.FLAG_IMMUTABLE
        )

        val stopIntent = Intent(this, ByeByeDpiVpnService::class.java).apply {
            action = ACTION_STOP
        }
        val stopPendingIntent = PendingIntent.getService(
            this, 1, stopIntent,
            PendingIntent.FLAG_UPDATE_CURRENT or PendingIntent.FLAG_IMMUTABLE
        )

        val uptimeFormatted = String.format("%02d:%02d", uptimeSeconds / 60, uptimeSeconds % 60)

        return NotificationCompat.Builder(this, NOTIFICATION_CHANNEL_ID)
            .setSmallIcon(R.drawable.ic_notification)
            .setContentTitle("ByeByeDPI Aktif")
            .setContentText("Mod: $presetName | Süre: $uptimeFormatted")
            .setPriority(NotificationCompat.PRIORITY_LOW)
            .setOngoing(true)
            .setContentIntent(openAppPendingIntent)
            .addAction(
                R.drawable.ic_notification,
                getString(R.string.notification_action_stop),
                stopPendingIntent
            )
            .build()
    }

    private fun createNotificationChannel() {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            val channel = NotificationChannel(
                NOTIFICATION_CHANNEL_ID,
                getString(R.string.notification_channel_name),
                NotificationManager.IMPORTANCE_LOW
            ).apply {
                description = getString(R.string.notification_channel_desc)
                setShowBadge(false)
            }
            val nm = getSystemService(Context.NOTIFICATION_SERVICE) as NotificationManager
            nm.createNotificationChannel(channel)
        }
    }

    override fun onDestroy() {
        stopVpnTunnel()
        serviceScope.cancel()
        super.onDestroy()
    }

    override fun onRevoke() {
        DpiEngineController.log("[VPN] Kullanıcı veya sistem VPN iznini iptal etti.")
        stopVpnTunnel()
        stopSelf()
        super.onRevoke()
    }
}
