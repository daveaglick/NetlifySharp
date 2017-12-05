using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace NetlifySharp.Operations
{
    public abstract class ResponseOperation<TResponse, TThis> : Operation<TThis>
        where TResponse : class
        where TThis : ResponseOperation<TResponse, TThis>
    {
        protected ResponseOperation(NetlifyClient client, Endpoint endpoint, HttpMethod method)
            : base(client, endpoint, method)
        {
        }

        public async Task<TResponse> SendAsync(CancellationToken cancellationToken = default(CancellationToken)) => 
            await GetResponseAsync<TResponse>(cancellationToken);

        protected override async Task<TResponse> ReadResponseAsync<TResponse>(HttpResponseMessage response) => 
            await ReadJsonResponseAsync<TResponse>(response);
    }
}