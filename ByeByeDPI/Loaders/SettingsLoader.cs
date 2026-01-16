using ByeByeDPI.Constants;
using ReaLTaiizor.Enum.Poison;
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
        public bool ShowInTaskbar { get; set; } = false;
        public bool AlwaysTopMost { get; set; } = true;
        public bool AutoHideWhenUnfocus { get; set; } = true;
        public ThemeStyle SelectedTheme { get; set; } = ThemeStyle.Default;
    }

	public static class SettingsLoader
	{
		public static SettingsModel Current { get; set; }

		public static void LoadSettings()
		{
			string path = AppConstants.AppSettingsPath;

			if (!File.Exists(path))
			{
				Current = new SettingsModel();
				Save();
				return;
			}
			try
			{
				string json = File.ReadAllText(path);
				Current = JsonSerializer.Deserialize<SettingsModel>(json) ?? new SettingsModel();
			}
			catch
			{
				Current = new SettingsModel();
				Save();
			}
		}

		public static void Save()
		{
			try
			{
				string path = AppConstants.AppSettingsPath;
				string folder = Path.GetDirectoryName(path);

				if (!Directory.Exists(folder))
					Directory.CreateDirectory(folder);

				string json = JsonSerializer.Serialize(Current, new JsonSerializerOptions { WriteIndented = true });
				File.WriteAllText(path, json);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Failed to save settings.\nError: {ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
