using System.Net.Http;
using System.Runtime.CompilerServices;
using NetlifySharp.Models;

namespace NetlifySharp.Operations.Sites
{
    public class ListSites : PagedOperation<Site[], ListSites>
    {
        internal ListSites(NetlifyClient client)
            : base(client, NetlifyClient.SitesEndpoint, HttpMethod.Get)
        {
        }
    }
}
