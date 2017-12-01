using System.Net.Http;
using System.Runtime.CompilerServices;
using NetlifySharp.Models;

namespace NetlifySharp.Operations
{
    public class ListSites : Operation<Site[], ListSites>
    {
        internal ListSites(NetlifyClient client)
            : base(client, NetlifyClient.SitesEndpoint, HttpMethod.Get)
        {
        }
    }
}
