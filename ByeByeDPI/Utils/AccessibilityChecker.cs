using ByeByeDPI.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ByeByeDPI.Utils
{
    public static class AccessibilityChecker
    {
        /// <summary>
        /// Checks the accessibility of a list of URLs.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="list"></param>
        /// <param name="onMessage"></param>
        /// <param name="onProgress"></param>
        /// <param name="onStatus"></param>
        /// <returns></returns>
        public static async Task CheckAsync(
            HttpClient client,
            List<CheckListWrapperModel> list,
            Action<string> onMessage,
            Action<int, int> onProgress,
            Action<string> onStatus)
        {
            int total = list.Count;
            int done = 0;

            foreach (var item in list)
            {
                onStatus?.Invoke($"Checking {item.Item.Name}...");

                bool accessible = false;
                try
                {
                    var url = item.Item.Url.StartsWith("www.")
                        ? item.Item.Url
                        : "www." + item.Item.Url;

                    var response = await client.GetAsync("https://" + url);
                    accessible = (int)response.StatusCode < 400;
                }
                catch { }

                item.IsAccesible = accessible;
                done++;

                onMessage?.Invoke($"{(accessible ? "✅" : "❌")} {item.Item.Name}");
                onProgress?.Invoke(done, total);
            }
        }
    }

}
