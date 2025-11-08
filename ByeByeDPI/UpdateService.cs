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
			UpdateDownloadUrl = $"https://github.com/alisariaslan/ByeByeDPI/releases/download/v{partialVersionStr}/ByeByeDPI_v{partialVersionStr}.zip";
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
				 HttpClient client = new HttpClient();
				var bytes = await client.GetByteArrayAsync(UpdateDownloadUrl);

				string fileName = Path.GetFileName(new Uri(UpdateDownloadUrl).LocalPath);
				string tempPath = Path.Combine(Path.GetTempPath(), fileName);

				File.WriteAllBytes(tempPath, bytes);
				MessageBox.Show($"Update downloaded to {tempPath}. The application will now close and run the update.",
								"Update Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
				client.Dispose();
				Process.Start(new ProcessStartInfo
				{
					FileName = tempPath,
					UseShellExecute = true
				});
				Application.Exit();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Failed to download or run update.\nError: {ex.Message}", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
