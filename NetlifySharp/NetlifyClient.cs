using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace NetlifySharp
{
    public class NetlifyClient
    {
        private const string ApiUrl = "https://api.netlify.com/api/v1";

        private readonly Endpoint SitesEndpoint = new Endpoint("sites");
        private readonly Endpoint FormsEndpoint = new Endpoint("forms");

        private HttpClient HttpClient = new HttpClient();
        
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

            HttpClient.DefaultRequestHeaders.Add("User-Agent", nameof(NetlifySharp));
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
        }

        internal async Task<TResponse> SendAsync<TResponse>(Endpoint endpoint)
            where TResponse : class =>
            await SendAsync<TResponse>(HttpMethod.Get, endpoint);

        internal async Task<TResponse> SendAsync<TResponse>(HttpMethod method, Endpoint endpoint)
            where TResponse : class
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{ApiUrl}/{endpoint}");
            HttpResponseMessage response = await HttpClient.SendAsync(request);
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(TResponse), new DataContractJsonSerializerSettings
            {
                DateTimeFormat = new DateTimeFormat("yyyy-MM-dd'T'HH:mm:ss.fffK")              
            });
            return serializer.ReadObject(await response.Content.ReadAsStreamAsync()) as TResponse;
        }

        internal string AppendId(string endpoint, string id, string paramName)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException($"{paramName} cannot be null or empty", paramName);
            }
            return $"{endpoint}/{id}";
        }

        public async Task<IList<Site>> GetSitesAsync() => await SendAsync<IList<Site>>(SitesEndpoint);

        public async Task<Site> GetSiteAsync(string siteId) => await SendAsync<Site>(SitesEndpoint.AppendId(siteId, nameof(siteId)));

        public async Task<IList<Form>> GetFormsAsync() => await SendAsync<IList<Form>>(FormsEndpoint);

        public async Task<IList<Form>> GetFormsAsync(string siteId) => 
            await SendAsync<IList<Form>>(SitesEndpoint.AppendId(siteId, nameof(siteId)).Append(FormsEndpoint));
    }
}
