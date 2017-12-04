using System.Net.Http;
using System.Runtime.CompilerServices;
using NetlifySharp.Models;

namespace NetlifySharp.Operations.Sites
{
    public class GetSite : Operation<Site, GetSite>
    {
        internal GetSite(NetlifyClient client, string siteId)
            : base(client, NetlifyClient.SitesEndpoint.Append(siteId, nameof(siteId)), HttpMethod.Get)
        {
        }
    }
}
