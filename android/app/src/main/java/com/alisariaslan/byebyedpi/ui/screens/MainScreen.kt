package com.alisariaslan.byebyedpi.ui.screens

import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.*
import androidx.compose.material3.*
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.alisariaslan.byebyedpi.core.VpnState
import com.alisariaslan.byebyedpi.core.VpnStatus
import com.alisariaslan.byebyedpi.data.DpiPreset
import com.alisariaslan.byebyedpi.ui.components.ConnectionButton
import com.alisariaslan.byebyedpi.ui.theme.*

@Composable
fun MainScreen(
    vpnStatus: VpnStatus,
    activePreset: DpiPreset,
    onToggleConnection: () -> Unit,
    onNavigateToPresets: () -> Unit,
    modifier: Modifier = Modifier
) {
    val scrollState = rememberScrollState()

    Column(
        modifier = modifier
            .fillMaxSize()
            .background(DarkBackground)
            .verticalScroll(scrollState)
            .padding(horizontal = 20.dp, vertical = 24.dp),
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        // App Header
        Row(
            modifier = Modifier.fillMaxWidth(),
            horizontalArrangement = Arrangement.SpaceBetween,
            verticalAlignment = Alignment.CenterVertically
        ) {
            Column {
                Text(
                    text = "ByeByeDPI",
                    fontSize = 26.sp,
                    fontWeight = FontWeight.Bold,
                    color = TextPrimary,
                    letterSpacing = 0.5.sp
                )
                Text(
                    text = "Android DPI Bypass Manager",
                    fontSize = 13.sp,
                    color = NeonCyan
                )
            }

            // Status Pill
            Surface(
                color = when (vpnStatus.state) {
                    VpnState.CONNECTED -> StatusGreenGlow
                    VpnState.CONNECTING, VpnState.DISCONNECTING -> Color(0x33FFD600)
                    VpnState.ERROR -> StatusRedGlow
                    VpnState.DISCONNECTED -> DarkSurfaceVariant
                },
                shape = RoundedCornerShape(20.dp),
                border = androidx.compose.foundation.BorderStroke(
                    1.dp,
                    when (vpnStatus.state) {
                        VpnState.CONNECTED -> StatusGreen
                        VpnState.CONNECTING, VpnState.DISCONNECTING -> StatusAmber
                        VpnState.ERROR -> StatusRed
                        VpnState.DISCONNECTED -> DarkBorder
                    }
                )
            ) {
                Row(
                    modifier = Modifier.padding(horizontal = 12.dp, vertical = 6.dp),
                    verticalAlignment = Alignment.CenterVertically
                ) {
                    Box(
                        modifier = Modifier
                            .size(8.dp)
                            .clip(RoundedCornerShape(4.dp))
                            .background(
                                when (vpnStatus.state) {
                                    VpnState.CONNECTED -> StatusGreen
                                    VpnState.CONNECTING, VpnState.DISCONNECTING -> StatusAmber
                                    VpnState.ERROR -> StatusRed
                                    VpnState.DISCONNECTED -> TextMuted
                                }
                            )
                    )
                    Spacer(modifier = Modifier.width(6.dp))
                    Text(
                        text = when (vpnStatus.state) {
                            VpnState.CONNECTED -> "KORUMALI"
                            VpnState.CONNECTING -> "BAĞLANIYOR"
                            VpnState.DISCONNECTING -> "DURUYOR"
                            VpnState.ERROR -> "HATA"
                            VpnState.DISCONNECTED -> "DEVRE DIŞI"
                        },
                        fontSize = 11.sp,
                        fontWeight = FontWeight.Bold,
                        color = when (vpnStatus.state) {
                            VpnState.CONNECTED -> StatusGreen
                            VpnState.CONNECTING, VpnState.DISCONNECTING -> StatusAmber
                            VpnState.ERROR -> StatusRed
                            VpnState.DISCONNECTED -> TextSecondary
                        }
                    )
                }
            }
        }

        Spacer(modifier = Modifier.height(36.dp))

        // Center Pulsing Connection Button
        ConnectionButton(
            state = vpnStatus.state,
            onClick = onToggleConnection
        )

        Spacer(modifier = Modifier.height(36.dp))

        // Active Preset Card
        Card(
            modifier = Modifier
                .fillMaxWidth()
                .clip(RoundedCornerShape(16.dp))
                .border(1.dp, DarkBorder, RoundedCornerShape(16.dp))
                .clickable { onNavigateToPresets() },
            colors = CardDefaults.cardColors(containerColor = DarkSurface)
        ) {
            Row(
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(18.dp),
                horizontalArrangement = Arrangement.SpaceBetween,
                verticalAlignment = Alignment.CenterVertically
            ) {
                Row(verticalAlignment = Alignment.CenterVertically) {
                    Surface(
                        color = DarkSurfaceVariant,
                        shape = RoundedCornerShape(10.dp),
                        modifier = Modifier.size(42.dp)
                    ) {
                        Box(contentAlignment = Alignment.Center) {
                            Icon(
                                imageVector = Icons.Default.Tune,
                                contentDescription = null,
                                tint = NeonCyan,
                                modifier = Modifier.size(22.dp)
                            )
                        }
                    }
                    Spacer(modifier = Modifier.width(14.dp))
                    Column {
                        Text(
                            text = "Aktif Mod",
                            fontSize = 12.sp,
                            color = TextMuted
                        )
                        Text(
                            text = activePreset.name,
                            fontSize = 15.sp,
                            fontWeight = FontWeight.SemiBold,
                            color = TextPrimary
                        )
                    }
                }

                Row(verticalAlignment = Alignment.CenterVertically) {
                    Text(
                        text = "Değiştir",
                        fontSize = 13.sp,
                        color = NeonCyan,
                        fontWeight = FontWeight.Medium
                    )
                    Icon(
                        imageVector = Icons.Default.ChevronRight,
                        contentDescription = null,
                        tint = NeonCyan,
                        modifier = Modifier.size(18.dp)
                    )
                }
            }
        }

        Spacer(modifier = Modifier.height(16.dp))

        // Statistics / Status Grid
        Row(
            modifier = Modifier.fillMaxWidth(),
            horizontalArrangement = Arrangement.spacedBy(12.dp)
        ) {
            // Uptime card
            Card(
                modifier = Modifier
                    .weight(1f)
                    .clip(RoundedCornerShape(16.dp))
                    .border(1.dp, DarkBorder, RoundedCornerShape(16.dp)),
                colors = CardDefaults.cardColors(containerColor = DarkSurface)
            ) {
                Column(modifier = Modifier.padding(16.dp)) {
                    Text(
                        text = "Çalışma Süresi",
                        fontSize = 12.sp,
                        color = TextMuted
                    )
                    Spacer(modifier = Modifier.height(6.dp))
                    val hours = vpnStatus.uptimeSeconds / 3600
                    val minutes = (vpnStatus.uptimeSeconds % 3600) / 60
                    val seconds = vpnStatus.uptimeSeconds % 60
                    Text(
                        text = if (vpnStatus.state == VpnState.CONNECTED) {
                            String.format("%02d:%02d:%02d", hours, minutes, seconds)
                        } else {
                            "--:--:--"
                        },
                        fontSize = 18.sp,
                        fontWeight = FontWeight.Bold,
                        color = if (vpnStatus.state == VpnState.CONNECTED) StatusGreen else TextSecondary
                    )
                }
            }

            // Mode Category Card
            Card(
                modifier = Modifier
                    .weight(1f)
                    .clip(RoundedCornerShape(16.dp))
                    .border(1.dp, DarkBorder, RoundedCornerShape(16.dp)),
                colors = CardDefaults.cardColors(containerColor = DarkSurface)
            ) {
                Column(modifier = Modifier.padding(16.dp)) {
                    Text(
                        text = "Kategori",
                        fontSize = 12.sp,
                        color = TextMuted
                    )
                    Spacer(modifier = Modifier.height(6.dp))
                    Text(
                        text = activePreset.category,
                        fontSize = 16.sp,
                        fontWeight = FontWeight.SemiBold,
                        color = ElectricViolet
                    )
                }
            }
        }

        Spacer(modifier = Modifier.height(16.dp))

        // Info Banner
        Card(
            modifier = Modifier
                .fillMaxWidth()
                .clip(RoundedCornerShape(16.dp))
                .border(1.dp, DarkBorder, RoundedCornerShape(16.dp)),
            colors = CardDefaults.cardColors(containerColor = DarkSurfaceVariant)
        ) {
            Row(
                modifier = Modifier.padding(14.dp),
                verticalAlignment = Alignment.CenterVertically
            ) {
                Icon(
                    imageVector = Icons.Default.Info,
                    contentDescription = null,
                    tint = NeonCyan,
                    modifier = Modifier.size(20.dp)
                )
                Spacer(modifier = Modifier.width(10.dp))
                Text(
                    text = "Bu uygulama uzak sunucuya bağlanmaz; DPI sansürünü telefonunuzda yerel paket manipülasyonu ile aşar.",
                    fontSize = 12.sp,
                    color = TextSecondary,
                    lineHeight = 16.sp
                )
            }
        }
    }
}
