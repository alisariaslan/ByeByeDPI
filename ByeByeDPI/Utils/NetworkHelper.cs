using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ByeByeDPI.Utils
{
    public static class NetworkHelper
    {
        // -------------------------
        // Public API
        // -------------------------

        public static async Task<(bool Success, string Message)> SetDNSToAutomaticAsync()
        {
            var adapter = await GetRealActiveInterfaceAsync();
            if (adapter == null)
                return (false, "❌ Active network adapter not found.");

            await RunNetshAsync(
                $"interface ip set dns name=\"{adapter}\" source=dhcp");

            await FlushDNSInternalAsync();

            return (true, "✅ DNS set to automatic (DHCP).");
        }

        public static async Task<(bool Success, string Message)> ApplyGoogleDNSAsync()
        {
            var adapter = await GetRealActiveInterfaceAsync();
            if (adapter == null)
                return (false, "❌ Active network adapter not found.");

            await RunNetshAsync(
                $"interface ip set dns name=\"{adapter}\" static 8.8.8.8 primary");

            await RunNetshAsync(
                $"interface ip add dns name=\"{adapter}\" 8.8.4.4 index=2");

            await FlushDNSInternalAsync();

            return (true, "✅ Google DNS applied successfully.");
        }

        public static async Task<(bool Success, string Message)> FlushDNSAsync()
        {
            await FlushDNSInternalAsync();
            return (true, "✅ DNS cache flushed.");
        }

        // -------------------------
        // Internal helpers
        // -------------------------

private static async Task<string?> GetRealActiveInterfaceAsync()
    {
        var psi = new ProcessStartInfo
        {
            FileName = "netsh",
            Arguments = "interface ip show addresses",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        };

        using var process = Process.Start(psi);
        string output = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();

        string? currentInterface = null;

        foreach (var line in output.Split(Environment.NewLine))
        {
            // Interface adı
            var ifaceMatch = Regex.Match(
                line,
                @"^Configuration for interface ""(.+)""");

            if (ifaceMatch.Success)
            {
                currentInterface = ifaceMatch.Groups[1].Value;
                continue;
            }

            // Default Gateway varsa → gerçek adaptör
            if (currentInterface != null &&
                line.TrimStart().StartsWith("Default Gateway", StringComparison.OrdinalIgnoreCase))
            {
                return currentInterface;
            }
        }

        return null;
    }


    private static async Task RunNetshAsync(string arguments)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "netsh",
                Arguments = arguments,
                UseShellExecute = true,
                Verb = "runas",
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            await process.WaitForExitAsync();
        }


        private static async Task FlushDNSInternalAsync()
        {
            var psi = new ProcessStartInfo
            {
                FileName = "ipconfig",
                Arguments = "/flushdns",
                UseShellExecute = true,
                Verb = "runas",
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            await process.WaitForExitAsync();
        }


       
    }
}
