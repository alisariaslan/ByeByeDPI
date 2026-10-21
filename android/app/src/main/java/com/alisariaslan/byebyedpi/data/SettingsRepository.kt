package com.alisariaslan.byebyedpi.data

import android.content.Context
import androidx.datastore.core.DataStore
import androidx.datastore.preferences.core.*
import androidx.datastore.preferences.preferencesDataStore
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.map

val Context.dataStore: DataStore<Preferences> by preferencesDataStore(name = "byebyedpi_settings")

class SettingsRepository(private val context: Context) {

    companion object {
        val KEY_SELECTED_PRESET_ID = stringPreferencesKey("selected_preset_id")
        val KEY_CUSTOM_ARGUMENTS = stringPreferencesKey("custom_arguments")
        val KEY_DNS_PROVIDER = stringPreferencesKey("dns_provider")
        val KEY_CUSTOM_DNS_PRIMARY = stringPreferencesKey("custom_dns_primary")
        val KEY_CUSTOM_DNS_SECONDARY = stringPreferencesKey("custom_dns_secondary")
        val KEY_AUTO_START_BOOT = booleanPreferencesKey("auto_start_boot")
        val KEY_SPLIT_TUNNEL_ENABLED = booleanPreferencesKey("split_tunnel_enabled")
        val KEY_SPLIT_TUNNEL_MODE = stringPreferencesKey("split_tunnel_mode")
        val KEY_SELECTED_PACKAGES = stringSetPreferencesKey("selected_packages")
    }

    val selectedPresetId: Flow<String> = context.dataStore.data.map { prefs ->
        prefs[KEY_SELECTED_PRESET_ID] ?: "turkey_balanced"
    }

    val customArguments: Flow<String> = context.dataStore.data.map { prefs ->
        prefs[KEY_CUSTOM_ARGUMENTS] ?: "--split 1 --disoob 1 --auto=torst"
    }

    val dnsProvider: Flow<DnsProvider> = context.dataStore.data.map { prefs ->
        val name = prefs[KEY_DNS_PROVIDER] ?: DnsProvider.CLOUDFLARE.name
        try {
            DnsProvider.valueOf(name)
        } catch (_: Exception) {
            DnsProvider.CLOUDFLARE
        }
    }

    val customDnsPrimary: Flow<String> = context.dataStore.data.map { prefs ->
        prefs[KEY_CUSTOM_DNS_PRIMARY] ?: "1.1.1.1"
    }

    val customDnsSecondary: Flow<String> = context.dataStore.data.map { prefs ->
        prefs[KEY_CUSTOM_DNS_SECONDARY] ?: "1.0.0.1"
    }

    val autoStartOnBoot: Flow<Boolean> = context.dataStore.data.map { prefs ->
        prefs[KEY_AUTO_START_BOOT] ?: false
    }

    val splitTunnelEnabled: Flow<Boolean> = context.dataStore.data.map { prefs ->
        prefs[KEY_SPLIT_TUNNEL_ENABLED] ?: false
    }

    val splitTunnelMode: Flow<SplitTunnelMode> = context.dataStore.data.map { prefs ->
        val mode = prefs[KEY_SPLIT_TUNNEL_MODE] ?: SplitTunnelMode.DISALLOW_SELECTED.name
        try {
            SplitTunnelMode.valueOf(mode)
        } catch (_: Exception) {
            SplitTunnelMode.DISALLOW_SELECTED
        }
    }

    val selectedPackages: Flow<Set<String>> = context.dataStore.data.map { prefs ->
        prefs[KEY_SELECTED_PACKAGES] ?: emptySet()
    }

    suspend fun setSelectedPresetId(id: String) {
        context.dataStore.edit { it[KEY_SELECTED_PRESET_ID] = id }
    }

    suspend fun setCustomArguments(args: String) {
        context.dataStore.edit { it[KEY_CUSTOM_ARGUMENTS] = args }
    }

    suspend fun setDnsProvider(provider: DnsProvider) {
        context.dataStore.edit { it[KEY_DNS_PROVIDER] = provider.name }
    }

    suspend fun setCustomDns(primary: String, secondary: String) {
        context.dataStore.edit {
            it[KEY_CUSTOM_DNS_PRIMARY] = primary
            it[KEY_CUSTOM_DNS_SECONDARY] = secondary
        }
    }

    suspend fun setAutoStartOnBoot(enabled: Boolean) {
        context.dataStore.edit { it[KEY_AUTO_START_BOOT] = enabled }
    }

    suspend fun setSplitTunnelEnabled(enabled: Boolean) {
        context.dataStore.edit { it[KEY_SPLIT_TUNNEL_ENABLED] = enabled }
    }

    suspend fun setSplitTunnelMode(mode: SplitTunnelMode) {
        context.dataStore.edit { it[KEY_SPLIT_TUNNEL_MODE] = mode.name }
    }

    suspend fun setSelectedPackages(packages: Set<String>) {
        context.dataStore.edit { it[KEY_SELECTED_PACKAGES] = packages }
    }
}
