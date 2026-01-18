using System;
using System.Linq;
using System.Management;
using System.Threading.Tasks;

namespace ByeByeDPI.Utils
{
    public static class NetworkHelper
    {
        /// <summary>
        /// Automatically finds the active NIC connected to the internet
        /// </summary>
        private static ManagementObject? GetActiveNetworkAdapter()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            foreach (ManagementObject mo in mc.GetInstances())
            {
                if ((bool)mo["IPEnabled"])
                {
                    var ips = (string[])mo["IPAddress"];
                    if (ips != null && ips.Any(ip => ip.StartsWith("192.") || ip.StartsWith("10.") || ip.StartsWith("172.")))
                    {
                        // Adapter connected to internet via IPv4 found
                        return mo;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Sets the DNS to obtain automatically (DHCP)
        /// </summary>
        public static async Task<(bool Success, string Message)> SetDNSToAutomaticAsync()
        {
            try
            {
                var mo = GetActiveNetworkAdapter();
                if (mo == null)
                    return (false, "❌ No active network adapter found.");

                ManagementBaseObject newDNS = mo.GetMethodParameters("SetDNSServerSearchOrder");
                newDNS["DNSServerSearchOrder"] = null;
                mo.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);

                return (true, "✅ DNS set to obtain automatically.");
            }
            catch (Exception ex)
            {
                return (false, $"❌ Failed to set DNS automatically: {ex.Message}");
            }
        }

        /// <summary>
        /// Applies Google DNS
        /// </summary>
        public static async Task<(bool Success, string Message)> ApplyGoogleDNSAsync()
        {
            try
            {
                var mo = GetActiveNetworkAdapter();
                if (mo == null)
                    return (false, "❌ No active network adapter found.");

                ManagementBaseObject newDNS = mo.GetMethodParameters("SetDNSServerSearchOrder");
                newDNS["DNSServerSearchOrder"] = new string[] { "8.8.8.8", "8.8.4.4" };
                mo.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);

                return (true, "✅ Google DNS applied successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"❌ Failed to apply Google DNS: {ex.Message}");
            }
        }

        /// <summary>
        /// Flushes DNS cache
        /// </summary>
        public static async Task<(bool Success, string Message)> FlushDNSAsync()
        {
            try
            {
                var process = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "ipconfig",
                        Arguments = "/flushdns",
                        Verb = "runas",
                        CreateNoWindow = true,
                        UseShellExecute = true,
                    }
                };
                process.Start();
                await process.WaitForExitAsync();

                return (true, "✅ DNS cache flushed successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"❌ Failed to flush DNS cache: {ex.Message}");
            }
        }

    }
}
