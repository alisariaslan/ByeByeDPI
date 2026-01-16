using ByeByeDPI.Constants;
using ByeByeDPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace ByeByeDPI
{


	public static class CheckListLoader
	{
		private static readonly Dictionary<string, string> DefaultCheckList = new Dictionary<string, string>
		{
			{ "Google", "google.com" },
			{ "Discord", "discord.com" },
			{ "GitHub", "github.com" },
			{ "Twitter", "twitter.com" },
			{ "Reddit", "reddit.com" },
			{ "LinkedIn", "linkedin.com" },
			{ "Instagram", "instagram.com" },
			{ "YouTube", "youtube.com" },
			{ "Pornhub", "pornhub.com" }
		};

		public static List<CheckListModel> LoadCheckList()
		{
			string path = AppConstants.CheckListPath;

			if (!File.Exists(path))
			{
				CreateDefaultFile(path);
				return ConvertToModelList(DefaultCheckList);
			}

			try
			{
				var json = File.ReadAllText(path);
				var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

				if (dict == null)
				{
					CreateDefaultFile(path);
					return ConvertToModelList(DefaultCheckList);
				}

				return ConvertToModelList(dict);
			}
			catch
			{
				CreateDefaultFile(path);
				return ConvertToModelList(DefaultCheckList);
			}
		}

		private static void CreateDefaultFile(string path)
		{
			try
			{
				var options = new JsonSerializerOptions { WriteIndented = true };
				string json = JsonSerializer.Serialize(DefaultCheckList, options);
				File.WriteAllText(path, json);
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					"Failed to create check list file.\nError: " + ex.Message,
					"Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			}
		}

		private static List<CheckListModel> ConvertToModelList(Dictionary<string, string> dict)
		{
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
	}
}
