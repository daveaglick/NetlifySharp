using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace NetlifySharp.Tests
{
    internal class TestApiClient : IApiClient
    {
        public Endpoint Endpoint => new Endpoint(string.Empty);

        public List<HttpRequestMessage> Requests { get; } = new List<HttpRequestMessage>();

        public Stream ResponseContent { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public TestApiClient(HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            ResponseContent = new MemoryStream();
            StatusCode = statusCode;
        }

        public TestApiClient WithStringResponseContent(string response)
        {
            ResponseContent = new MemoryStream(Encoding.UTF8.GetBytes(response));
            return this;
        }

        public TestApiClient WithResourceResposeContent(string embeddedResource)
        {
            ResponseContent = typeof(TestApiClient).Assembly.GetManifestResourceStream($"{nameof(NetlifySharp)}.{nameof(Tests)}.{embeddedResource}");
            return this;
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests.Add(request);
            HttpResponseMessage responseMessage = new HttpResponseMessage(StatusCode);
            responseMessage.Content = new StreamContent(ResponseContent);
            return Task.FromResult(responseMessage);
        }
    }
}
