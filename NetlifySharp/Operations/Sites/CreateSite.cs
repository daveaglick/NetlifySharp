using System.Net.Http;
using NetlifySharp.Models;
using System;

namespace NetlifySharp.Operations.Sites
{
    public class CreateSite : Operation<Site, CreateSite>
    {
        private readonly SiteCreate _siteCreate;
        private bool? _configureDns;

        internal CreateSite(NetlifyClient client, SiteCreate siteCreate)
            : base(client, NetlifyClient.SitesEndpoint, HttpMethod.Post)
        {
            _siteCreate = siteCreate ?? throw new System.ArgumentNullException(nameof(siteCreate));
        }

        public CreateSite WithConfigureDns(bool configureDns)
        {
            _configureDns = configureDns;
            return this;
        }

        protected override HttpRequestMessage GetRequest()
        {
            HttpRequestMessage request = base.GetRequest();
            if(_configureDns.HasValue)
            {
                UriBuilder uri = new UriBuilder(request.RequestUri);
                uri.Query = $"?configure_dns={_configureDns.Value.ToString().ToLower()}";
                request.RequestUri = uri.Uri;
            }

            request.Content = new JsonContent(Client, _siteCreate);

            return request;
        }
    }
}
