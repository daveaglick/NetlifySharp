using Newtonsoft.Json;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System;
using NetlifySharp.Operations;

[assembly: InternalsVisibleTo("NetlifySharp.Tests")]

namespace NetlifySharp
{
    public class NetlifyClient
    {
        internal static Endpoint SitesEndpoint = new Endpoint("sites");
        internal static Endpoint FormsEndpoint = new Endpoint("forms");

        internal JsonSerializer Serializer { get; } = new JsonSerializer();
        internal IApiClient ApiClient { get; }

        public NetlifyClient(string accessToken)
            : this(new ApiClient(accessToken))
        {
        }

        internal NetlifyClient(IApiClient apiClient)
        {
            Serializer.ContractResolver = new ClientContractResolver(this);
            Serializer.MissingMemberHandling = MissingMemberHandling.Ignore;
            ApiClient = apiClient;
        }

        public Action<HttpRequestMessage> RequestHandler { get; set; }

        public Action<HttpResponseMessage> ResponseHandler { get; set; }

        public ListSites ListSites() => new ListSites(this);

        public GetSite GetSite(string siteId) => new GetSite(this, siteId);

        public ListForms ListForms() => new ListForms(this);

        public ListSiteForms ListSiteForms(string siteId) => new ListSiteForms(this, siteId);
    }
}
