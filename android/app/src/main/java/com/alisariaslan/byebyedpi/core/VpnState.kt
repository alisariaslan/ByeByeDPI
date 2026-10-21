package com.alisariaslan.byebyedpi.core

enum class VpnState {
    DISCONNECTED,
    CONNECTING,
    CONNECTED,
    DISCONNECTING,
    ERROR
}

data class VpnStatus(
    val state: VpnState = VpnState.DISCONNECTED,
    val activePresetName: String = "",
    val activeArguments: String = "",
    val uptimeSeconds: Long = 0,
    val txBytes: Long = 0,
    val rxBytes: Long = 0,
    val errorMessage: String? = null
)
