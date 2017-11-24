using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NetlifySharp
{
    public class NetlifyClient
    {
        private const string ApiUrl = "https://api.netlify.com/api/v1";

        private readonly HttpClient _httpClient = new HttpClient();

        private readonly Endpoint _sitesEndpoint = new Endpoint("sites");
        private readonly Endpoint _formsEndpoint = new Endpoint("forms");

        private readonly JsonSerializer _serializer = new JsonSerializer();

        public NetlifyClient(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentException("Access token cannot be null or empty", nameof(accessToken));
            }
            if (accessToken.Any(x => char.IsWhiteSpace(x) || char.IsControl(x)))
            {
                throw new ArgumentException("Invalid access token", nameof(accessToken));
            }

            _httpClient.DefaultRequestHeaders.Add("User-Agent", nameof(NetlifySharp));
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            _serializer.ContractResolver = new CustomContractResolver(this);
            _serializer.MissingMemberHandling = MissingMemberHandling.Ignore;
        }

        private async Task<TResponse> SendAsync<TResponse>(Endpoint endpoint)
            where TResponse : class =>
            await SendAsync<TResponse>(HttpMethod.Get, endpoint);

        private async Task<TResponse> SendAsync<TResponse>(HttpMethod method, Endpoint endpoint)
            where TResponse : class
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{ApiUrl}/{endpoint}");
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            
            using (Stream stream = await response.Content.ReadAsStreamAsync())
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

        // Internal for testing
        internal string AppendId(string endpoint, string id, string paramName)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException($"{paramName} cannot be null or empty", paramName);
            }
            return $"{endpoint}/{id}";
        }

        public async Task<Site[]> GetSitesAsync() => await SendAsync<Site[]>(_sitesEndpoint);

        public async Task<Site> GetSiteAsync(string siteId) => await SendAsync<Site>(_sitesEndpoint.AppendId(siteId, nameof(siteId)));

        public async Task<Form[]> GetFormsAsync() => await SendAsync<Form[]>(_formsEndpoint);

        public async Task<Form[]> GetFormsAsync(string siteId) => 
            await SendAsync<Form[]>(_sitesEndpoint.AppendId(siteId, nameof(siteId)).Append(_formsEndpoint));
    }
}
