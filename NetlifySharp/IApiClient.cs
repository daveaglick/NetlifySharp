using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace NetlifySharp
{
    internal interface IApiClient
    {
        Task<Stream> SendAndReadAsync(HttpMethod method, Endpoint endpoint);
    }
}
