using System.Net.Http;
using NetlifySharp.Models;

namespace NetlifySharp.Operations.Sites
{
    public class CreateSite : ResponseOperation<Site, CreateSite>
    {
        internal CreateSite(NetlifyClient client, SiteCreate siteCreate)
            : base(client, NetlifyClient.SitesEndpoint, HttpMethod.Post)
        {
            Body = siteCreate;
        }

        public CreateSite WithConfigureDns(bool configureDns)
        {
            Query.Add("configure_dns", configureDns.ToString().ToLower());
            return this;
        }
    }
}
