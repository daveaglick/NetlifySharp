using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System;
using System.Threading;

namespace NetlifySharp.Operations
{
    public abstract class Operation<TResponse, TThis>
        where TResponse : class
        where TThis : Operation<TResponse, TThis>
    {
        public NetlifyClient Client { get; }
        public Endpoint Endpoint { get; }
        public HttpMethod Method { get; }
        public Action<HttpRequestMessage> RequestHandler { get; set; }
        public Action<HttpResponseMessage> ResponseHandler { get; set; }

        protected Operation(NetlifyClient client, Endpoint endpoint, HttpMethod method)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
            Endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
            Method = method ?? HttpMethod.Get;
        }

        public TThis WithRequestHandler(Action<HttpRequestMessage> requestHandler)
        {
            RequestHandler = requestHandler;
            return (TThis)this;
        }

        public TThis WithResponseHandler(Action<HttpResponseMessage> responseHandler)
        {
            ResponseHandler = responseHandler;
            return (TThis)this;
        }

        public async Task<TResponse> SendAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Generate the request
            HttpRequestMessage request = GetRequest();
            Client.RequestHandler?.Invoke(request);
            RequestHandler?.Invoke(request);

            // Send it to the API and handle response
            HttpResponseMessage response = await Client.ApiClient.SendAsync(request, cancellationToken);
            ProcessResponse(response);
            Client.ResponseHandler?.Invoke(response);
            ResponseHandler?.Invoke(response);

            // Read the content
            return await ReadResponse(response);
        }

        protected virtual HttpRequestMessage GetRequest() =>
            new HttpRequestMessage(Method, Client.ApiClient.Endpoint.Append(Endpoint));

        protected virtual void ProcessResponse(HttpResponseMessage response) =>
            response.EnsureSuccessStatusCode();

        protected virtual async Task<TResponse> ReadResponse(HttpResponseMessage response)
        {
            using (Stream stream = await response.Content.ReadAsStreamAsync())
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    using (JsonTextReader jsonReader = new JsonTextReader(streamReader))
                    {
                        return Client.Serializer.Deserialize<TResponse>(jsonReader);
                    }
                }
            }
        }
    }
}
