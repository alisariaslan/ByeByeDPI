using ByeByeDPI.Constants;
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
		private static readonly Dictionary<string, string> DefaultParams = new Dictionary<string, string>
		{
			{ "default", "-5 --set-ttl 5 --dns-addr 77.88.8.8 --dns-port 1253 --dnsv6-addr 2a02:6b8::feed:0ff --dnsv6-port 1253" },
			{ "ttl3", "--set-ttl 3" },
			{ "mode5", "-5" },
			{ "ttl3_full", "--set-ttl 3 --dns-addr 77.88.8.8 --dns-port 1253 --dnsv6-addr 2a02:6b8::feed:0ff --dnsv6-port 1253" },
			{ "mode5_dns", "-5 --dns-addr 77.88.8.8 --dns-port 1253 --dnsv6-addr 2a02:6b8::feed:0ff --dnsv6-port 1253" },
			{ "mode9_dns", "-9 --dns-addr 77.88.8.8 --dns-port 1253 --dnsv6-addr 2a02:6b8::feed:0ff --dnsv6-port 1253" },
			{ "mode9", "-9" }
		};

		public static List<ParamModel> LoadParams()
		{
			if (!File.Exists(AppConstants.ParamsPath))
			{
				CreateDefaultFile(AppConstants.ParamsPath);
				return ConvertToModelList(DefaultParams);
			}
			try
			{
				var json = File.ReadAllText(AppConstants.ParamsPath);
				var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
				if (dict == null)
				{
					CreateDefaultFile(AppConstants.ParamsPath);
					return ConvertToModelList(DefaultParams);
				}
				return ConvertToModelList(dict);
			}
			catch
			{
				CreateDefaultFile(AppConstants.ParamsPath);
				return ConvertToModelList(DefaultParams);
			}
		}

		private static void CreateDefaultFile(string path)
		{
			try
			{
				var options = new JsonSerializerOptions { WriteIndented = true };
				string json = JsonSerializer.Serialize(DefaultParams, options);
				File.WriteAllText(path, json);
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					"Failed to create parameters file.\nError: " + ex.Message,
					"Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			}
		}

		private static List<ParamModel> ConvertToModelList(Dictionary<string, string> dict)
		{
			var list = new List<ParamModel>();
			foreach (var kvp in dict)
			{
				list.Add(new ParamModel
				{
					Name = kvp.Key,
					Value = kvp.Value
				});
			}
			return list;
		}
	}
}
