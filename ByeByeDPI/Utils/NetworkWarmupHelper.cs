using System.Net.Http;
using System.Threading.Tasks;

namespace ByeByeDPI.Utils
{
    public static class NetworkWarmupHelper
    {
        public static async Task<bool> WaitAsync(
            HttpClient client,
            string testUrl = "https://www.google.com",
            int maxAttempts = 3,
            int delayMs = 2000)
        {
            for (int i = 0; i < maxAttempts; i++)
            {
                try
                {
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
