using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ByeByeDPI.Utils
{
    public static class NetworkWarmupHelper
    {
        /// <summary>
        /// Waits until the network is responsive by making test requests
        /// </summary>
        /// <param name="client"></param>
        /// <param name="onStatus"></param>
        /// <param name="testUrl"></param>
        /// <param name="maxAttempts"></param>
        /// <param name="delayMs"></param>
        /// <returns></returns>
        public static async Task<bool> WaitAsync(
            HttpClient client,
            Action<string> onStatus,
            string testUrl = "https://www.google.com",
            int maxAttempts = 3,
            int delayMs = 2000)
        {
            for (int i = 0; i < maxAttempts; i++)
            {
                try
                {
                    onStatus?.Invoke($"Network Warmup (Attempt {i + 1}/{maxAttempts})..."); // Bilgi gönder
                    await Task.Delay(delayMs);
                    var response = await client.GetAsync(testUrl);
                    if ((int)response.StatusCode < 400)
                        return true;
                }
                catch { }
            }
            return false;
        }
    }

}
