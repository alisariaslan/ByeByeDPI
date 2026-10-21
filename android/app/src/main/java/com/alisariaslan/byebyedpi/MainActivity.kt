package com.alisariaslan.byebyedpi

import android.Manifest
import android.app.Activity
import android.content.Intent
import android.content.pm.PackageManager
import android.net.VpnService
import android.os.Build
import android.os.Bundle
import android.widget.Toast
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Scaffold
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.core.content.ContextCompat
import com.alisariaslan.byebyedpi.core.ByeByeDpiVpnService
import com.alisariaslan.byebyedpi.core.VpnState
import com.alisariaslan.byebyedpi.data.*
import com.alisariaslan.byebyedpi.ui.components.AppBottomNavigation
import com.alisariaslan.byebyedpi.ui.components.Screen
import com.alisariaslan.byebyedpi.ui.screens.*
import com.alisariaslan.byebyedpi.ui.theme.ByeByeDPITheme
import kotlinx.coroutines.launch

class MainActivity : ComponentActivity() {

    private lateinit var settingsRepository: SettingsRepository

    private val vpnPermissionLauncher = registerForActivityResult(
        ActivityResultContracts.StartActivityForResult()
    ) { result ->
        if (result.resultCode == Activity.RESULT_OK) {
            ByeByeDpiVpnService.start(this)
        } else {
            Toast.makeText(this, "VPN bağlantı izni reddedildi.", Toast.LENGTH_SHORT).show()
        }
    }

    private val notificationPermissionLauncher = registerForActivityResult(
        ActivityResultContracts.RequestPermission()
    ) { /* Permission granted or denied */ }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        settingsRepository = SettingsRepository(this)

        checkNotificationPermission()

        setContent {
            ByeByeDPITheme {
                val vpnStatus by ByeByeDpiVpnService.vpnStatus.collectAsState()
                val selectedPresetId by settingsRepository.selectedPresetId.collectAsState(initial = "turkey_balanced")
                val customArgs by settingsRepository.customArguments.collectAsState(initial = "")
                val dnsProvider by settingsRepository.dnsProvider.collectAsState(initial = DnsProvider.CLOUDFLARE)
                val customDnsPrimary by settingsRepository.customDnsPrimary.collectAsState(initial = "1.1.1.1")
                val customDnsSecondary by settingsRepository.customDnsSecondary.collectAsState(initial = "1.0.0.1")
                val autoStartOnBoot by settingsRepository.autoStartOnBoot.collectAsState(initial = false)
                val splitTunnelEnabled by settingsRepository.splitTunnelEnabled.collectAsState(initial = false)
                val splitTunnelMode by settingsRepository.splitTunnelMode.collectAsState(initial = SplitTunnelMode.DISALLOW_SELECTED)
                val selectedPackages by settingsRepository.selectedPackages.collectAsState(initial = emptySet())

                var currentRoute by remember { mutableStateOf(Screen.Home.route) }
                val coroutineScope = rememberCoroutineScope()
                val activePreset = remember(selectedPresetId) { PresetRepository.getById(selectedPresetId) }

                Scaffold(
                    modifier = Modifier.fillMaxSize(),
                    bottomBar = {
                        AppBottomNavigation(
                            currentRoute = currentRoute,
                            onNavigate = { currentRoute = it }
                        )
                    }
                ) { innerPadding ->
                    when (currentRoute) {
                        Screen.Home.route -> {
                            MainScreen(
                                vpnStatus = vpnStatus,
                                activePreset = activePreset,
                                onToggleConnection = { toggleVpnConnection(vpnStatus.state) },
                                onNavigateToPresets = { currentRoute = Screen.Presets.route },
                                modifier = Modifier.padding(innerPadding)
                            )
                        }
                        Screen.Presets.route -> {
                            PresetsScreen(
                                selectedPresetId = selectedPresetId,
                                customArguments = customArgs,
                                onSelectPreset = { id ->
                                    coroutineScope.launch { settingsRepository.setSelectedPresetId(id) }
                                },
                                onSaveCustomArguments = { args ->
                                    coroutineScope.launch { settingsRepository.setCustomArguments(args) }
                                },
                                modifier = Modifier.padding(innerPadding)
                            )
                        }
                        Screen.SplitTunnel.route -> {
                            SplitTunnelScreen(
                                enabled = splitTunnelEnabled,
                                mode = splitTunnelMode,
                                selectedPackages = selectedPackages,
                                onToggleEnabled = { enabled ->
                                    coroutineScope.launch { settingsRepository.setSplitTunnelEnabled(enabled) }
                                },
                                onToggleMode = { mode ->
                                    coroutineScope.launch { settingsRepository.setSplitTunnelMode(mode) }
                                },
                                onTogglePackage = { pkg, isSelected ->
                                    coroutineScope.launch {
                                        val newSet = if (isSelected) {
                                            selectedPackages + pkg
                                        } else {
                                            selectedPackages - pkg
                                        }
                                        settingsRepository.setSelectedPackages(newSet)
                                    }
                                },
                                modifier = Modifier.padding(innerPadding)
                            )
                        }
                        Screen.Logs.route -> {
                            LogsScreen(modifier = Modifier.padding(innerPadding))
                        }
                        Screen.Settings.route -> {
                            SettingsScreen(
                                dnsProvider = dnsProvider,
                                customDnsPrimary = customDnsPrimary,
                                customDnsSecondary = customDnsSecondary,
                                autoStartOnBoot = autoStartOnBoot,
                                onSelectDns = { provider ->
                                    coroutineScope.launch { settingsRepository.setDnsProvider(provider) }
                                },
                                onSaveCustomDns = { p, s ->
                                    coroutineScope.launch { settingsRepository.setCustomDns(p, s) }
                                },
                                onToggleAutoStart = { auto ->
                                    coroutineScope.launch { settingsRepository.setAutoStartOnBoot(auto) }
                                },
                                modifier = Modifier.padding(innerPadding)
                            )
                        }
                    }
                }
            }
        }
    }

    private fun toggleVpnConnection(currentState: VpnState) {
        if (currentState == VpnState.CONNECTED || currentState == VpnState.CONNECTING) {
            ByeByeDpiVpnService.stop(this)
        } else {
            val vpnIntent = VpnService.prepare(this)
            if (vpnIntent != null) {
                vpnPermissionLauncher.launch(vpnIntent)
            } else {
                ByeByeDpiVpnService.start(this)
            }
        }
    }

    private fun checkNotificationPermission() {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.TIRAMISU) {
            if (ContextCompat.checkSelfPermission(this, Manifest.permission.POST_NOTIFICATIONS)
                != PackageManager.PERMISSION_GRANTED
            ) {
                notificationPermissionLauncher.launch(Manifest.permission.POST_NOTIFICATIONS)
            }
        }
    }
}
