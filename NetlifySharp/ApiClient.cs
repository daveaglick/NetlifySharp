using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NetlifySharp
{
    internal class ApiClient : IApiClient
    {
        public Endpoint Endpoint { get; } = new Endpoint("https://api.netlify.com/api/v1");

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

        public async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken) =>
            await _httpClient.SendAsync(request, cancellationToken);
    }
}
