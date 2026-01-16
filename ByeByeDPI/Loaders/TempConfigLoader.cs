using ByeByeDPI.Constants;
using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace ByeByeDPI
{
	public class TempConfigModel
	{
		public bool AdminPriviligesRequested { get; set; } = false;
        public DateTime LastUpdateCheck { get; set; }

        public int MainFormX { get; set; } = -1;
        public int MainFormY { get; set; } = -1;
        public int MainFormWidth { get; set; } = -1;
        public int MainFormHeight { get; set; } = -1;

	}

	public static class TempConfigLoader
	{
		public static TempConfigModel Current { get; set; }

		public static void LoadSettings()
		{
			string path = AppConstants.TempConfigsPath;

			if (!File.Exists(path))
			{
				Current = new TempConfigModel();
				Save();
				return;
			}

			try
			{
				string json = File.ReadAllText(path);
				Current = JsonSerializer.Deserialize<TempConfigModel>(json) ?? new TempConfigModel();
			}
			catch
			{
				// Dosya bozuksa default ile oluştur
				Current = new TempConfigModel();
				Save();
			}
		}

		public static void Save()
		{
			try
			{
				string path = AppConstants.TempConfigsPath;
				string folder = Path.GetDirectoryName(path);

				if (!Directory.Exists(folder))
					Directory.CreateDirectory(folder);

				string json = JsonSerializer.Serialize(Current, new JsonSerializerOptions { WriteIndented = true });
				File.WriteAllText(path, json);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Failed to save temp configs.\nError: {ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
