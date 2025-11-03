using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace ByeByeDPI
{
	public class ParamModel
	{
		public string Name { get; set; }
		public string Value { get; set; }
	}

	public static class ParamsLoader
	{
		public static List<ParamModel> LoadParams(string jsonFileName)
		{
			string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, jsonFileName);

			if (!File.Exists(jsonPath))
				return new List<ParamModel>();

			try
			{
				var json = File.ReadAllText(jsonPath);
				var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
				var list = new List<ParamModel>();

				if (dict != null)
				{
					foreach (var kvp in dict)
					{
						list.Add(new ParamModel
						{
							Name = kvp.Key,
							Value = kvp.Value
						});
					}
				}

				return list;
			}
			catch (Exception ex)
			{
				var result = MessageBox.Show(
					"Failed to load parameters configuration.\nError: " + ex.Message +
					"\n\nDo you want to delete the file and restart the application?",
					"Error Loading Parameters",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Error);

				if (result == DialogResult.Yes)
				{
					try
					{
						File.Delete(jsonPath);
						MessageBox.Show("The parameters file has been deleted.\nPlease restart the application.",
										"File Deleted",
										MessageBoxButtons.OK,
										MessageBoxIcon.Information);
					}
					catch (Exception deleteEx)
					{
						MessageBox.Show("Failed to delete the parameters file.\nError: " + deleteEx.Message,
										"Delete Failed",
										MessageBoxButtons.OK,
										MessageBoxIcon.Error);
					}

					Application.Exit();
				}

				return new List<ParamModel>();
			}
		}
	}
}
