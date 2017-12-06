using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NetlifySharp
{
    internal interface IApiClient
    {
        Endpoint Endpoint { get; }
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
    }
}
