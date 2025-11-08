using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;


namespace ByeByeDPI
{
	public class TempConfigModel
	{
		public bool AdminPriviligesRequested { get; set; } = false;

	}

	public static class TempConfigLoader
	{
		public static TempConfigModel Current { get; set; }

		public static void Reset_AdminPriviliges_Request()
		{
			Current.AdminPriviligesRequested = new TempConfigModel().AdminPriviligesRequested;
			Save();
		}

		public static void LoadSettings()
		{
			try
			{
				string json = File.ReadAllText(Constants.TempConfigsPath);
				Current = JsonSerializer.Deserialize<TempConfigModel>(json) ?? new TempConfigModel();
				return;
			}
			catch (Exception ex)
			{
				var result = MessageBox.Show(
					$"Failed to load temp configs.\nError: {ex.Message}\n\nDo you want to delete the corrupted temp configs file and restart the application?",
					"Temp Configs Load Error",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Error);

				if (result == DialogResult.Yes)
				{
					try
					{
						File.Delete(Constants.TempConfigsPath);
						MessageBox.Show("Temp configs deleted. Please restart the application.", "File Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					catch (Exception deleteEx)
					{
						MessageBox.Show($"Failed to delete temp configs.\nError: {deleteEx.Message}", "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}

					Application.Exit();
				}

				Current = new TempConfigModel();
				return;
			}
		}

		public static void Save()
		{
			try
			{
				string json = JsonSerializer.Serialize(Current, new JsonSerializerOptions { WriteIndented = true });
				File.WriteAllText(Constants.TempConfigsPath, json);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Failed to save temp configs.\nError: {ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
