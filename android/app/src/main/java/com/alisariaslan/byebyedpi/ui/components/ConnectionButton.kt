package com.alisariaslan.byebyedpi.ui.components

import androidx.compose.animation.animateColorAsState
import androidx.compose.animation.core.*
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.interaction.MutableInteractionSource
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.PowerSettingsNew
import androidx.compose.material3.Icon
import androidx.compose.material3.Text
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.draw.scale
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.alisariaslan.byebyedpi.core.VpnState
import com.alisariaslan.byebyedpi.ui.theme.*

@Composable
fun ConnectionButton(
    state: VpnState,
    onClick: () -> Unit,
    modifier: Modifier = Modifier
) {
    val isConnected = state == VpnState.CONNECTED
    val isConnecting = state == VpnState.CONNECTING || state == VpnState.DISCONNECTING

    val primaryColor by animateColorAsState(
        targetValue = when (state) {
            VpnState.CONNECTED -> StatusGreen
            VpnState.CONNECTING, VpnState.DISCONNECTING -> StatusAmber
            VpnState.ERROR -> StatusRed
            VpnState.DISCONNECTED -> NeonCyan
        },
        animationSpec = tween(500),
        label = "buttonColor"
    )

    val glowColor by animateColorAsState(
        targetValue = when (state) {
            VpnState.CONNECTED -> StatusGreenGlow
            VpnState.CONNECTING, VpnState.DISCONNECTING -> Color(0x33FFD600)
            VpnState.ERROR -> StatusRedGlow
            VpnState.DISCONNECTED -> StatusCyanGlow
        },
        animationSpec = tween(500),
        label = "glowColor"
    )

    // Pulsing animation when connected or connecting
    val infiniteTransition = rememberInfiniteTransition(label = "pulse")
    val pulseScale by infiniteTransition.animateFloat(
        initialValue = 1f,
        targetValue = if (isConnected || isConnecting) 1.08f else 1f,
        animationSpec = infiniteRepeatable(
            animation = tween(1200, easing = FastOutSlowInEasing),
            repeatMode = RepeatMode.Reverse
        ),
        label = "pulseScale"
    )

    Box(
        contentAlignment = Alignment.Center,
        modifier = modifier.size(240.dp)
    ) {
        // Outer glowing pulse ring
        Box(
            modifier = Modifier
                .size(210.dp)
                .scale(pulseScale)
                .clip(CircleShape)
                .background(glowColor)
        )

        // Middle ring with border
        Box(
            modifier = Modifier
                .size(185.dp)
                .clip(CircleShape)
                .background(DarkSurfaceVariant)
                .border(2.dp, primaryColor.copy(alpha = 0.5f), CircleShape)
        )

        // Core interactive button
        Box(
            contentAlignment = Alignment.Center,
            modifier = Modifier
                .size(150.dp)
                .clip(CircleShape)
                .background(
                    Brush.radialGradient(
                        colors = listOf(
                            primaryColor.copy(alpha = 0.25f),
                            DarkSurface
                        )
                    )
                )
                .border(2.dp, primaryColor, CircleShape)
                .clickable(
                    interactionSource = remember { MutableInteractionSource() },
                    indication = null,
                    onClick = onClick
                )
        ) {
            Column(
                horizontalAlignment = Alignment.CenterHorizontally,
                verticalArrangement = Arrangement.Center
            ) {
                Icon(
                    imageVector = Icons.Default.PowerSettingsNew,
                    contentDescription = "Bağlan",
                    tint = primaryColor,
                    modifier = Modifier.size(52.dp)
                )
                Spacer(modifier = Modifier.height(6.dp))
                Text(
                    text = when (state) {
                        VpnState.CONNECTED -> "DURDUR"
                        VpnState.CONNECTING -> "BAĞLANIYOR"
                        VpnState.DISCONNECTING -> "DURUYOR"
                        VpnState.ERROR -> "TEKRAR DENE"
                        VpnState.DISCONNECTED -> "BAŞLAT"
                    },
                    color = TextPrimary,
                    fontSize = 13.sp,
                    fontWeight = FontWeight.Bold,
                    letterSpacing = 1.sp
                )
            }
        }
    }
}
