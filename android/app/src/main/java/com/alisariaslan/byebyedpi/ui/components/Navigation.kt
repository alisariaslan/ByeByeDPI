package com.alisariaslan.byebyedpi.ui.components

import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.*
import androidx.compose.material3.*
import androidx.compose.runtime.Composable
import androidx.compose.ui.graphics.vector.ImageVector
import androidx.compose.ui.unit.dp
import com.alisariaslan.byebyedpi.ui.theme.*

sealed class Screen(val route: String, val title: String, val icon: ImageVector) {
    object Home : Screen("home", "Ana Sayfa", Icons.Default.Shield)
    object Presets : Screen("presets", "Modlar", Icons.Default.Tune)
    object SplitTunnel : Screen("split_tunnel", "Uygulamalar", Icons.Default.Apps)
    object Logs : Screen("logs", "Kayıtlar", Icons.Default.Terminal)
    object Settings : Screen("settings", "Ayarlar", Icons.Default.Settings)
}

@Composable
fun AppBottomNavigation(
    currentRoute: String,
    onNavigate: (String) -> Unit
) {
    val screens = listOf(
        Screen.Home,
        Screen.Presets,
        Screen.SplitTunnel,
        Screen.Logs,
        Screen.Settings
    )

    NavigationBar(
        containerColor = DarkSurface,
        tonalElevation = 8.dp
    ) {
        screens.forEach { screen ->
            val selected = currentRoute == screen.route
            NavigationBarItem(
                selected = selected,
                onClick = { onNavigate(screen.route) },
                icon = {
                    Icon(
                        imageVector = screen.icon,
                        contentDescription = screen.title
                    )
                },
                label = { Text(screen.title) },
                colors = NavigationBarItemDefaults.colors(
                    selectedIconColor = DarkBackground,
                    selectedTextColor = NeonCyan,
                    indicatorColor = NeonCyan,
                    unselectedIconColor = TextSecondary,
                    unselectedTextColor = TextSecondary
                )
            )
        }
    }
}
