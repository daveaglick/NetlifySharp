using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using NetlifySharp.Models;
using System;
using System.Threading;

[assembly: InternalsVisibleTo("NetlifySharp.Tests")]

namespace NetlifySharp
{
    public class NetlifyClient
    {  
        internal static Endpoint SitesEndpoint = new Endpoint("sites");
        internal static Endpoint FormsEndpoint = new Endpoint("forms");

        private readonly JsonSerializer _serializer = new JsonSerializer();
        private readonly IApiClient _apiClient;

        public NetlifyClient(string accessToken)
            : this(new ApiClient(accessToken))
        {
        }

        internal NetlifyClient(IApiClient apiClient)
        {
            _serializer.ContractResolver = new ClientContractResolver(this);
            _serializer.MissingMemberHandling = MissingMemberHandling.Ignore;
            _apiClient = apiClient;
        }

        private async Task<TResponse> SendAndDeserializeAsync<TResponse>(Endpoint endpoint,
            Action<HttpRequestMessage> customizeRequest = null,
            CancellationToken cancellationToken = default(CancellationToken))
            where TResponse : class =>
            await SendAndDeserializeAsync<TResponse>(HttpMethod.Get, endpoint, customizeRequest, cancellationToken);

        private async Task<TResponse> SendAndDeserializeAsync<TResponse>(HttpMethod method,
            Endpoint endpoint,
            Action<HttpRequestMessage> customizeRequest = null,
            CancellationToken cancellationToken = default(CancellationToken))
            where TResponse : class
        {
            using (Stream stream = await _apiClient.SendAndReadAsync(method, endpoint, customizeRequest, cancellationToken))
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    using (JsonTextReader jsonReader = new JsonTextReader(streamReader))
                    {
                        return _serializer.Deserialize<TResponse>(jsonReader);
                    }
                }
            }
        }

        public async Task<Site[]> GetSitesAsync(
            Action<HttpRequestMessage> customizeRequest = null,
            CancellationToken cancellationToken = default(CancellationToken)) =>
            await SendAndDeserializeAsync<Site[]>(SitesEndpoint, customizeRequest, cancellationToken);

        public async Task<Site> GetSiteAsync(
            string siteId,
            Action<HttpRequestMessage> customizeRequest = null,
            CancellationToken cancellationToken = default(CancellationToken)) =>
            await SendAndDeserializeAsync<Site>(SitesEndpoint.Append(siteId, nameof(siteId)), customizeRequest, cancellationToken);

        public async Task<Form[]> GetFormsAsync(
            Action<HttpRequestMessage> customizeRequest = null,
            CancellationToken cancellationToken = default(CancellationToken)) =>
            await SendAndDeserializeAsync<Form[]>(FormsEndpoint, customizeRequest, cancellationToken);

        public async Task<Form[]> GetFormsAsync(
            string siteId,
            Action<HttpRequestMessage> customizeRequest = null,
            CancellationToken cancellationToken = default(CancellationToken)) => 
            await SendAndDeserializeAsync<Form[]>(SitesEndpoint.Append(siteId, nameof(siteId)).Append(FormsEndpoint), customizeRequest, cancellationToken);
    }
}
