using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace ByeByeDPI
{
	public class CheckListModel
	{
		public string Name { get; set; }
		public string Url { get; set; }
		public bool Accessible { get; set; } = false;
	}

	public static class CheckListLoader
	{
		public static List<CheckListModel> LoadCheckList(string jsonPath)
		{
			if (!File.Exists(jsonPath))
				return new List<CheckListModel>();

			try
			{
				var json = File.ReadAllText(jsonPath);
				var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
				var list = new List<CheckListModel>();

				foreach (var kvp in dict)
				{
					list.Add(new CheckListModel
					{
						Name = kvp.Key,
						Url = kvp.Value
					});
				}

				return list;
			}
			catch (Exception ex)
			{
				// Error handling: offer to delete corrupted file and restart
				var result = MessageBox.Show(
					"Failed to load check list configuration.\nError: " + ex.Message +
					"\n\nDo you want to delete the file and restart the application?",
					"Error Loading Check List",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Error);

				if (result == DialogResult.Yes)
				{
					try
					{
						File.Delete(jsonPath);
						MessageBox.Show("The configuration file has been deleted.\nPlease restart the application.",
										"File Deleted",
										MessageBoxButtons.OK,
										MessageBoxIcon.Information);
					}
					catch (Exception deleteEx)
					{
						MessageBox.Show("Failed to delete the configuration file.\nError: " + deleteEx.Message,
										"Delete Failed",
										MessageBoxButtons.OK,
										MessageBoxIcon.Error);
					}

					// Optionally exit the app so user can restart
					Application.Exit();
				}

				return new List<CheckListModel>();
			}
		}
	}
}
