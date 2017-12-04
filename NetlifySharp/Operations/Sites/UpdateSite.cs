using System.Net.Http;
using NetlifySharp.Models;
using System;

namespace NetlifySharp.Operations.Sites
{
    public class UpdateSite : ResponseOperation<Site, UpdateSite>
    {
        internal UpdateSite(NetlifyClient client, string siteId, SiteSetup siteSetup)
            : base(client, NetlifyClient.SitesEndpoint.Append(siteId), new HttpMethod("PATCH"))
        {
            if (string.IsNullOrEmpty(siteId))
            {
                throw new ArgumentException("A site ID must be provided", nameof(siteId));
            }
            if (siteSetup == null)
            {
                throw new ArgumentNullException(nameof(siteSetup));
            }
        }
    }
}
