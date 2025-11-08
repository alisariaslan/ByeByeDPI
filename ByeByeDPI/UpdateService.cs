using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ByeByeDPI
{
	public class UpdateInfo
	{
		public string Version { get; set; }
		public string Notes { get; set; }
	}

	public static class UpdateService
	{
		private const string UpdateCheckUrl = "https://raw.githubusercontent.com/alisariaslan/ByeByeDPI/main/latest_version.json";
		private static string UpdateDownloadUrl { get; set; }

		public static void SetUpdateDownloadUrl(string partialVersionStr)
		{
			UpdateDownloadUrl = "https://github.com/alisariaslan/ByeByeDPI/releases/download/latest/ByeByeDPI_Installer.exe";
		}

		public static async Task<UpdateInfo> CheckForUpdateAsync(string currentVersion)
		{
			try
			{
				 HttpClient client = new HttpClient();
				var json = await client.GetStringAsync(UpdateCheckUrl);
				var info = JsonSerializer.Deserialize<UpdateInfo>(json);
				if (info == null)
					throw new Exception("Failed to parse update information.");
				SetUpdateDownloadUrl(info.Version);
				Version latest = new Version(info.Version);
				Version current = new Version(currentVersion);
				client.Dispose();
				if(latest > current)
					return info;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Failed to check updates.\nError: {ex.Message}", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return null;
		}

		public static async Task DownloadAndRunUpdateAsync()
		{
			try
			{
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

					Process.Start(new ProcessStartInfo
					{
						FileName = targetPath,
						UseShellExecute = true
					});

					Application.Exit();
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
