using Newtonsoft.Json;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System;
using NetlifySharp.Operations.Sites;
using NetlifySharp.Operations.Forms;
using NetlifySharp.Models;

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

        // Operations
        public ListSites ListSites() => new ListSites(this);
        public CreateSite CreateSite(SiteCreate siteCreate) => new CreateSite(this, siteCreate);
        public GetSite GetSite(string siteId) => new GetSite(this, siteId);
        public DeleteSite DeleteSite(string siteId) => new DeleteSite(this, siteId);
        public UpdateSite UpdateSite(string siteId, SiteSetup siteSetup) => new UpdateSite(this, siteId, siteSetup);
        public ListForms ListForms() => new ListForms(this);
        public ListSiteForms ListSiteForms(string siteId) => new ListSiteForms(this, siteId);
    }
}
