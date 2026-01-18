using System;
using System.Net;
using System.Net.Http;

namespace ByeByeDPI.Utils
{
    public static class HttpClientFactory
    {
        public static HttpClient CreateNoRedirectClient(int timeoutSeconds = 5)
        {
            return new HttpClient(new HttpClientHandler
            {
                UseCookies = false,
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.None
            })
            {
                Timeout = TimeSpan.FromSeconds(timeoutSeconds)
            };
        }
    }

}
