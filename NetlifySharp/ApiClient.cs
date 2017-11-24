using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NetlifySharp
{
    internal class ApiClient : IApiClient
    {
        private static Endpoint ApiEndpoint = new Endpoint("https://api.netlify.com/api/v1");

        private readonly HttpClient _httpClient = new HttpClient();

        public ApiClient(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentException("Access token cannot be null or empty", nameof(accessToken));
            }
            if (accessToken.Any(x => char.IsWhiteSpace(x) || char.IsControl(x)))
            {
                throw new ArgumentException("Invalid access token", nameof(accessToken));
            }

            _httpClient.DefaultRequestHeaders.Add("User-Agent", nameof(NetlifySharp));
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
        }

        public async Task<Stream> SendAndReadAsync(HttpMethod method, Endpoint endpoint)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ApiEndpoint.Append(endpoint).ToString());
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsStreamAsync();
        }
    }
}
