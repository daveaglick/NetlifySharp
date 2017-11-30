using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace NetlifySharp.Tests
{
    internal class TestApiClient : IApiClient
    {
        public Stream Response { get; set; }

        public List<(HttpMethod method, Endpoint endpoint)> SendAndReadCalls { get; } = new List<(HttpMethod method, Endpoint endpoint)>();

        public TestApiClient()
        {
            Response = new MemoryStream();
        }

        public TestApiClient WithStringResponse(string response)
        {
            Response = new MemoryStream(Encoding.UTF8.GetBytes(response));
            return this;
        }

        public TestApiClient WithResourceRespose(string embeddedResource)
        {
            Response = typeof(TestApiClient).Assembly.GetManifestResourceStream($"{nameof(NetlifySharp)}.{nameof(Tests)}.{embeddedResource}");
            return this;
        }

        public Task<Stream> SendAndReadAsync(
            HttpMethod method,
            Endpoint endpoint,
            Action<HttpRequestMessage> customizeRequest = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            SendAndReadCalls.Add((method, endpoint));
            return Task.FromResult(Response);
        }
    }
}
