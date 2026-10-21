package com.alisariaslan.byebyedpi.data

enum class DnsProvider(val title: String, val primary: String, val secondary: String) {
    CLOUDFLARE("Cloudflare DNS", "1.1.1.1", "1.0.0.1"),
    GOOGLE("Google DNS", "8.8.8.8", "8.8.4.4"),
    ADGUARD("AdGuard DNS (Reklam Engelleyici)", "94.140.14.14", "94.140.15.15"),
    QUAD9("Quad9 DNS", "9.9.9.9", "149.112.112.112"),
    CUSTOM("Özel DNS", "", "")
}

data class DpiPreset(
    val id: String,
    val name: String,
    val description: String,
    val category: String,
    val arguments: String,
    val isRecommended: Boolean = false
)

object PresetRepository {
    val presets = listOf(
        DpiPreset(
            id = "turkey_balanced",
            name = "Türkiye Standart (Önerilen)",
            description = "Tüm servis sağlayıcılar (TurkNet, TTNET, Superonline) için dengeli TLS ve SNI parçalama. Minimum hız kaybı.",
            category = "Türkiye Özel",
            arguments = "--split 1 --disoob 1 --auto=torst",
            isRecommended = true
        ),
        DpiPreset(
            id = "turkey_aggressive",
            name = "Türkiye Agresif (Superonline / TT)",
            description = "Derin DPI uygulayan ağlar için 2 baytlık ClientHello parçalama ve TLS kayıt bölme yöntemi.",
            category = "Türkiye Özel",
            arguments = "--split 2 --disoob 2 --tlsrec 1+s --auto=torst"
        ),
        DpiPreset(
            id = "discord_fix",
            name = "Discord & Voice Özel",
            description = "Discord ses sunucuları, kanallar ve medya akışı bağlantı problemleri için optimize edilmiş strateji.",
            category = "Uygulama Özel",
            arguments = "--split 1 --oob 1 --auto=torst"
        ),
        DpiPreset(
            id = "turknet_light",
            name = "TurkNet / Vodafone Hafif",
            description = "Hafif SNI parçalama ile sıfır gecikme ve tam bağlantı hızı.",
            category = "Türkiye Özel",
            arguments = "--split 1"
        ),
        DpiPreset(
            id = "custom",
            name = "Özel Parametreler",
            description = "Gelişmiş kullanıcılar için kendi komut satırı argümanlarını belirleme modu.",
            category = "Gelişmiş",
            arguments = ""
        )
    )

    fun getById(id: String): DpiPreset {
        return presets.find { it.id == id } ?: presets.first()
    }
}

enum class SplitTunnelMode {
    DISALLOW_SELECTED, // Seçilen uygulamalar tüneli kullanmasın (Baypas)
    ALLOW_SELECTED     // Yalnızca seçilen uygulamalar tüneli kullansın
}

data class AppItem(
    val packageName: String,
    val appName: String,
    val isSelected: Boolean = false
)
