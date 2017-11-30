using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NetlifySharp
{
    internal interface IApiClient
    {
        Task<Stream> SendAndReadAsync(
            HttpMethod method,
            Endpoint endpoint,
            Action<HttpRequestMessage> customizeRequest = null,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
