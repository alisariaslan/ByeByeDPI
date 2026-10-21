package com.alisariaslan.byebyedpi

import android.app.Application
import com.alisariaslan.byebyedpi.core.DpiEngineController

class ByeByeDPIApp : Application() {
    override fun onCreate() {
        super.onCreate()
        DpiEngineController.log("[APP] ByeByeDPI Android başlatıldı.")
    }
}
