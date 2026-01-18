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
            Dictionary<string, string> fileParams = new Dictionary<string, string>();

            // Dosya varsa oku
            if (File.Exists(AppConstants.ParamsPath))
            {
                try
                {
                    var json = File.ReadAllText(AppConstants.ParamsPath);
                    fileParams = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
                }
                catch
                {
                    // Okuma başarısızsa, boş dict ile devam et
                    fileParams = new Dictionary<string, string>();
                }
            }

            // Eksik default parametreleri ekle
            bool updated = false;
            foreach (var kvp in DefaultParams)
            {
                if (!fileParams.ContainsKey(kvp.Key))
                {
                    fileParams[kvp.Key] = kvp.Value;
                    updated = true;
                }
            }

            // Dosya eksik parametrelerle güncellendiyse tekrar yaz
            if (updated || !File.Exists(AppConstants.ParamsPath))
            {
                try
                {
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string json = JsonSerializer.Serialize(fileParams, options);
                    File.WriteAllText(AppConstants.ParamsPath, json);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Failed to update parameters file.\nError: " + ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }

            return ConvertToModelList(fileParams);
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
