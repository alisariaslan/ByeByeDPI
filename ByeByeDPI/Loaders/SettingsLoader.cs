using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace ByeByeDPI
{
	public class SettingsModel
	{
		public string ChosenParam { get; set; } = "";
		public bool HideToTray { get; set; } = false;
		public bool CheckUpdates { get; set; } = true;
		public bool StartWithWindows { get; set; } = false;
	}

	public static class SettingsLoader
	{
		public static SettingsModel Current { get; set; }

		public static void LoadSettings()
		{
			try
			{
				string json = File.ReadAllText(Constants.AppSettingsPath);
				Current =  JsonSerializer.Deserialize<SettingsModel>(json) ?? new SettingsModel();
				return;
			}
			catch (Exception ex)
			{
				var result = MessageBox.Show(
					$"Failed to load settings.\nError: {ex.Message}\n\nDo you want to delete the corrupted settings file and restart the application?",
					"Settings Load Error",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Error);

				if (result == DialogResult.Yes)
				{
					try
					{
						File.Delete(Constants.AppSettingsPath);
						MessageBox.Show("Settings file deleted. Please restart the application.", "File Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					catch (Exception deleteEx)
					{
						MessageBox.Show($"Failed to delete settings file.\nError: {deleteEx.Message}", "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}

					Application.Exit();
				}

				Current = new SettingsModel();
				return;
			}
		}

		public static void Save()
		{
			try
			{
				string json = JsonSerializer.Serialize(Current, new JsonSerializerOptions { WriteIndented = true });
				File.WriteAllText(Constants.AppSettingsPath, json);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Failed to save settings.\nError: {ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
