package com.alisariaslan.byebyedpi.receiver

import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent
import com.alisariaslan.byebyedpi.core.ByeByeDpiVpnService
import com.alisariaslan.byebyedpi.core.DpiEngineController
import com.alisariaslan.byebyedpi.data.SettingsRepository
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.flow.first
import kotlinx.coroutines.launch

class BootReceiver : BroadcastReceiver() {
    override fun onReceive(context: Context, intent: Intent) {
        if (intent.action == Intent.ACTION_BOOT_COMPLETED || intent.action == "android.intent.action.QUICKBOOT_POWERON") {
            CoroutineScope(Dispatchers.IO).launch {
                val settingsRepo = SettingsRepository(context)
                val autoStart = settingsRepo.autoStartOnBoot.first()
                if (autoStart) {
                    DpiEngineController.log("[BOOT] Cihaz açıldı, otomatik başlatma devrede...")
                    ByeByeDpiVpnService.start(context)
                }
            }
        }
    }
}
