using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using NetlifySharp.Models;

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

        private async Task<TResponse> SendAsync<TResponse>(Endpoint endpoint)
            where TResponse : class =>
            await SendAsync<TResponse>(HttpMethod.Get, endpoint);

        private async Task<TResponse> SendAsync<TResponse>(HttpMethod method, Endpoint endpoint)
            where TResponse : class
        {
            using (Stream stream = await _apiClient.SendAndReadAsync(method, endpoint))
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

        public async Task<Site[]> GetSitesAsync() => await SendAsync<Site[]>(SitesEndpoint);

        public async Task<Site> GetSiteAsync(string siteId) => await SendAsync<Site>(SitesEndpoint.Append(siteId, nameof(siteId)));

        public async Task<Form[]> GetFormsAsync() => await SendAsync<Form[]>(FormsEndpoint);

        public async Task<Form[]> GetFormsAsync(string siteId) => 
            await SendAsync<Form[]>(SitesEndpoint.Append(siteId, nameof(siteId)).Append(FormsEndpoint));
    }
}
