using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ByeByeDPI.Utils
{
    public static class AppVersionHelper
    {
        // Not: Assembly'yi yalnızca bir kez alıyoruz (performans için)
        private static readonly Assembly EntryAssembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();

        // Bu yardımcı sınıfın çalıştığından emin olmak için MainForm yerine Assembly.GetExecutingAssembly() kullanıldı.
        // Eğer uygulama başlangıcından eminseniz, üstteki satır yeterlidir.

        /// <summary>
        /// Sürüm formatında base version döner (Örn: 1.2.4)
        /// Projedeki <Version> etiketini yansıtan AssemblyInformationalVersion'ı kullanır.
        /// </summary>
        public static string GetBaseVersion()
        {
            try
            {
                // 1) AssemblyInformationalVersion'dan ana sürümü al (Bu, doğrudan <Version> etiketine eşittir)
                var infoAttr = EntryAssembly.GetCustomAttributes(false)
                    .OfType<AssemblyInformationalVersionAttribute>()
                    .FirstOrDefault();

                if (infoAttr != null && !string.IsNullOrEmpty(infoAttr.InformationalVersion))
                {
                    // Artık InformationalVersion, <Version> (1.2.4) olduğu için hash temizlemeye gerek yok,
                    // ancak yine de ek metadata'yı (örn: '1.2.4-beta') temizleyelim.
                    var cleanVersion = infoAttr.InformationalVersion
                        .Split('+')[0]
                        .Split('-')[0]
                        .Trim();

                    if (Version.TryParse(cleanVersion, out var version))
                    {
                        // Sadece ilk 3 bileşeni al
                        return $"{version.Major}.{version.Minor}.{version.Build}";
                    }
                }

                // 2) Assembly Version Fallback
                var ver = EntryAssembly.GetName().Version;
                if (ver != null && ver.Build >= 0)
                    return $"{ver.Major}.{ver.Minor}.{ver.Build}";
            }
            catch { }

            return "0.0.0";
        }

        /// <summary>
        /// Build numarasını döner (Örn: 14)
        /// ClickOnce'ın kullandığı FileVersion'ın son bileşenini veya Revision'ı dener.
        /// </summary>
        public static int GetBuildNumber()
        {
            try
            {
                var location = EntryAssembly.Location;

                // 1) FileVersionInfo (Eğer dosya sürümü set edilmişse en güvenilir 4. bileşen kaynağı)
                if (!string.IsNullOrEmpty(location))
                {
                    var fvi = FileVersionInfo.GetVersionInfo(location);
                    // FilePrivatePart, FileVersion'daki 4. bileşeni (Revision) temsil eder.
                    if (fvi.FilePrivatePart > 0)
                        return fvi.FilePrivatePart;
                }

                // 2) Assembly Version'dan Revision fallback (SDK stili projelerde 4. bileşen buraya düşer)
                var ver = EntryAssembly.GetName().Version;
                if (ver != null && ver.Revision >= 0)
                    return ver.Revision;
            }
            catch { }

            return 0;
        }

        /// <summary>
        /// Uygulama sürümünü "1.2.4 (14)" formatında döndürür.
        /// </summary>
        public static string GetAppVersion()
        {
            var baseVersion = GetBaseVersion();
            var build = GetBuildNumber();

            // Sadece ApplicationRevision (14) büyük 0 ise parantez içinde gösteririz.
            return build > 0
                ? $"{baseVersion} ({build})"
                : baseVersion;
        }
    }
}