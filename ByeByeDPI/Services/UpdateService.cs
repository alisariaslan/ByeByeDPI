using ByeByeDPI.Constants;
using ByeByeDPI.Core;
using ByeByeDPI.Models;
using ByeByeDPI.Utils;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ByeByeDPI
{

    public class UpdateService
    {
        private const string UpdateCheckUrl = "https://raw.githubusercontent.com/alisariaslan/ByeByeDPI/main/latest_version_v2.json";
        private string UpdateDownloadUrl { get; set; }

        public void SetUpdateDownloadUrl()
        {
            UpdateDownloadUrl = "https://github.com/alisariaslan/ByeByeDPI/releases/download/latest/ByeByeDPI_Installer.exe";
        }

        // Event Definition: Notifies the UI when an update is found
        public event EventHandler<UpdateInfo> UpdateAvailable;

        private static CancellationTokenSource _cts;
        private bool _isChecking;

        /// <summary>
        /// Starts the periodic update check in the background.
        /// </summary>
        public void StartPeriodicCheck(int currentBuild)
        {
            if (_isChecking) return;
            _isChecking = true;
            _cts = new CancellationTokenSource();

            Task.Run((Func<Task>)(async () =>
            {
                try
                {
                    while (!_cts.Token.IsCancellationRequested)
                    {
                        if (SettingsLoader.Current.CheckUpdates)
                        {
                            var now = DateTime.UtcNow;
                            var last = TempConfigLoader.Current.LastUpdateCheck;

                            if (last == default || (now - last) >= AppConstants.ApplicationUpdateInterval)
                            {
                                var update = await CheckForUpdateAsync(true);
                                if (update != null)
                                    UpdateAvailable?.Invoke(this, update.UpdateInfo);
                            }
                        }

                        await Task.Delay(TimeSpan.FromMinutes(5), _cts.Token);
                    }
                }
                catch (OperationCanceledException) { }
                finally { _isChecking = false; }
            }), _cts.Token);
        }


        public void StopPeriodicCheck()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }

        /// <summary>
        /// Fetches the latest version info from GitHub and compares it with the current version.
        /// </summary>
        public async Task<UpdateInfoWrapper> CheckForUpdateAsync(bool silent = false)
        {
            try
            {
                using var client = new HttpClient();
                var json = await client.GetStringAsync(UpdateCheckUrl);
                var info = JsonSerializer.Deserialize<UpdateInfo>(json);

                if (info == null)
                    throw new Exception("Failed to parse update information.");

                SetUpdateDownloadUrl();

                var latestBuildNumber = info.BuildNumber;
                var currentBuildNumber = AppVersionHelper.GetBuildNumber();

                TempConfigLoader.Current.LastUpdateCheck = DateTime.UtcNow;
                TempConfigLoader.Save();

                if (latestBuildNumber > currentBuildNumber)
                    return new UpdateInfoWrapper(true, updateInfo: info);

                return new UpdateInfoWrapper(true); // Güncel
            }
            catch (Exception ex)
            {
                return new UpdateInfoWrapper(false, ex.Message);
            }
        }

        /// <summary>
        /// Downloads the installer and executes it, then closes the current application.
        /// </summary>
        public async Task DownloadAndRunUpdateAsync()
        {
            try
            {
                // Antivirus warning for the user
                MessageBox.Show(
                    "Before proceeding, please temporarily disable your antivirus software.\n" +
                    "Some antivirus programs may falsely block the update installer.",
                    "Antivirus Warning",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                using (HttpClient client = new HttpClient())
                {
                    var bytes = await client.GetByteArrayAsync(UpdateDownloadUrl);
                    string fileName = Path.GetFileName(new Uri(UpdateDownloadUrl).LocalPath);

                    // Target the user's Downloads folder
                    string downloadsPath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                        "Downloads"
                    );
                    string targetPath = Path.Combine(downloadsPath, fileName);

                    File.WriteAllBytes(targetPath, bytes);

                    MessageBox.Show(
                        $"Update downloaded to:\n{targetPath}\n\nThe application will now close and start the installer.",
                        "Update Ready",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    // Launch the installer
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = targetPath,
                        UseShellExecute = true
                    });

                    // Shutdown the current app instance
                    TrayApplicationContext.Instance?.ExitApplication();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to download or run the update.\n\nError: {ex.Message}",
                    "Update Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}