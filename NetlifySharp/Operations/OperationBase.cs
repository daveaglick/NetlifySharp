using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using NetlifySharp.Models;

namespace NetlifySharp.Operations
{
    public abstract class OperationBase<TThis>
        where TThis : OperationBase<TThis>
    {
        public NetlifyClient Client { get; }
        public Endpoint Endpoint { get; }
        public HttpMethod Method { get; }

        public object Body { get; set; }
        public IDictionary<string, string> Query { get; } = new Dictionary<string, string>();

        public Action<HttpRequestMessage> RequestHandler { get; set; }
        public Action<HttpResponseMessage> ResponseHandler { get; set; }

        protected OperationBase(NetlifyClient client, Endpoint endpoint, HttpMethod method)
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
        
        protected virtual HttpRequestMessage GetRequest(CancellationToken cancellationToken)
        {
            string uriString = Client.ApiClient.Endpoint.Append(Endpoint)
                + (Query.Count > 0
                    ? "?" + string.Join("&", Query.Select(x => $"{WebUtility.UrlEncode(x.Key)}={WebUtility.UrlEncode(x.Value)}"))
                    : string.Empty);
            HttpRequestMessage request = new HttpRequestMessage(Method, uriString);
            if(Body != null)
            {
                request.Content = new JsonContent(Client, Body);
            }
            return request;
        }

        protected async Task<HttpResponseMessage> GetResponseAsync(CancellationToken cancellationToken)
        {
            // Generate the request
            using (HttpRequestMessage request = GetRequest(cancellationToken))
            {
                Client.RequestHandler?.Invoke(request);
                RequestHandler?.Invoke(request);

                // Send it to the API and handle response
                using (HttpResponseMessage response = await Client.ApiClient.SendAsync(request, cancellationToken))
                {
                    await ProcessResponse(response);
                    Client.ResponseHandler?.Invoke(response);
                    ResponseHandler?.Invoke(response);
                    return response;
                }
            }
        }

        protected virtual async Task ProcessResponse(HttpResponseMessage response)
        {
            if(!response.IsSuccessStatusCode)
            {
                Error error = await ReadJsonResponse<Error>(response);
                throw new ErrorResponseException(error, response.StatusCode);
            }
        }

        protected async Task<TModel> ReadJsonResponse<TModel>(HttpResponseMessage response)
        {
            using (Stream stream = await response.Content.ReadAsStreamAsync())
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    using (JsonTextReader jsonReader = new JsonTextReader(streamReader))
                    {
                        return Client.Serializer.Deserialize<TModel>(jsonReader);
                    }
                }
            }
        }
    }
}