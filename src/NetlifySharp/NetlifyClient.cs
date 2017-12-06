using Newtonsoft.Json;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System;
using NetlifySharp.Operations.Sites;
using NetlifySharp.Operations.Forms;
using NetlifySharp.Models;
using System.IO;

[assembly: InternalsVisibleTo("NetlifySharp.Tests")]

namespace NetlifySharp
{
    public class NetlifyClient
    {
        internal static Endpoint SitesEndpoint = new Endpoint("sites");
        internal static Endpoint FormsEndpoint = new Endpoint("forms");

        internal JsonSerializer Serializer { get; } = new JsonSerializer
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };

        internal IApiClient ApiClient { get; }

        public NetlifyClient(string accessToken)
            : this(new ApiClient(accessToken))
        {
        }

        internal NetlifyClient(IApiClient apiClient)
        {
            Serializer.ContractResolver = new ClientContractResolver(this);
            ApiClient = apiClient;
        }

        public Action<HttpRequestMessage> RequestHandler { get; set; }
        public Action<HttpResponseMessage> ResponseHandler { get; set; }

        // Operations
        public ListSites ListSites() => new ListSites(this);
        public CreateSite CreateSite(SiteSetup siteSetup) => new CreateSite(this, siteSetup);
        public GetSite GetSite(string siteId) => new GetSite(this, siteId);
        public DeleteSite DeleteSite(string siteId) => new DeleteSite(this, siteId);
        public UpdateSite UpdateSite(string siteId, SiteSetup siteSetup) => new UpdateSite(this, siteId, siteSetup);
        public UpdateSite UpdateSite(string siteId, Stream zipStream) => new UpdateSite(this, siteId, zipStream);
        public UpdateSite UpdateSite(string siteId, string directory) => new UpdateSite(this, siteId, directory);
        public ListForms ListForms() => new ListForms(this);
        public ListSiteForms ListSiteForms(string siteId) => new ListSiteForms(this, siteId);
    }
}
