using System.Net.Http;
using NetlifySharp.Models;

namespace NetlifySharp.Operations.Sites
{
    public class CreateSite : ResponseOperation<Site, CreateSite>
    {
        internal CreateSite(NetlifyClient client, SiteSetup siteSetup)
            : base(client, NetlifyClient.SitesEndpoint, HttpMethod.Post)
        {
            Body = siteSetup;
        }

        public CreateSite WithConfigureDns(bool configureDns)
        {
            Query.Add("configure_dns", configureDns.ToString().ToLower());
            return this;
        }
    }
}
