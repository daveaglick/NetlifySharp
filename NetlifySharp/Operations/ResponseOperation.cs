using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace NetlifySharp.Operations
{
    public abstract class ResponseOperation<TResponse, TThis> : OperationBase<TThis>
        where TResponse : class
        where TThis : ResponseOperation<TResponse, TThis>
    {
        protected ResponseOperation(NetlifyClient client, Endpoint endpoint, HttpMethod method)
            : base(client, endpoint, method)
        {
        }

        public async Task<TResponse> SendAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            HttpResponseMessage response = await GetResponseAsync(cancellationToken);
            return await ReadResponse(response);
        }

        protected virtual async Task<TResponse> ReadResponse(HttpResponseMessage response) => await ReadJsonResponse<TResponse>(response);
    }
}