package com.alisariaslan.byebyedpi.core

import android.os.Build
import android.service.quicksettings.Tile
import android.service.quicksettings.TileService
import androidx.annotation.RequiresApi
import kotlinx.coroutines.*
import kotlinx.coroutines.flow.collectLatest

@RequiresApi(Build.VERSION_CODES.N)
class QuickTileService : TileService() {

    private val serviceScope = CoroutineScope(Dispatchers.Main + SupervisorJob())
    private var observeJob: Job? = null

    override fun onStartListening() {
        super.onStartListening()
        observeJob = serviceScope.launch {
            ByeByeDpiVpnService.vpnStatus.collectLatest { status ->
                updateTile(status.state)
            }
        }
    }

    override fun onStopListening() {
        observeJob?.cancel()
        observeJob = null
        super.onStopListening()
    }

    override fun onClick() {
        super.onClick()
        val currentStatus = ByeByeDpiVpnService.vpnStatus.value.state
        if (currentStatus == VpnState.CONNECTED || currentStatus == VpnState.CONNECTING) {
            ByeByeDpiVpnService.stop(this)
        } else {
            ByeByeDpiVpnService.start(this)
        }
    }

    private fun updateTile(state: VpnState) {
        val tile = qsTile ?: return
        when (state) {
            VpnState.CONNECTED -> {
                tile.state = Tile.STATE_ACTIVE
                tile.label = "ByeByeDPI: Aktif"
            }
            VpnState.CONNECTING -> {
                tile.state = Tile.STATE_ACTIVE
                tile.label = "Bağlanıyor..."
            }
            VpnState.DISCONNECTING -> {
                tile.state = Tile.STATE_INACTIVE
                tile.label = "Kapatılıyor..."
            }
            VpnState.ERROR -> {
                tile.state = Tile.STATE_INACTIVE
                tile.label = "Hata Oluştu"
            }
            VpnState.DISCONNECTED -> {
                tile.state = Tile.STATE_INACTIVE
                tile.label = "ByeByeDPI: Kapalı"
            }
        }
        tile.updateTile()
    }

    override fun onDestroy() {
        serviceScope.cancel()
        super.onDestroy()
    }
}
