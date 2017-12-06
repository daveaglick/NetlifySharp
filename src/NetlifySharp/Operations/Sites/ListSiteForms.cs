using System.Net.Http;
using System.Runtime.CompilerServices;
using NetlifySharp.Models;

namespace NetlifySharp.Operations.Sites
{
    public class ListSiteForms : PagedOperation<Form[], ListSiteForms>
    {
        internal ListSiteForms(NetlifyClient client, string siteId)
            : base(client, NetlifyClient.SitesEndpoint.Append(siteId).Append(NetlifyClient.FormsEndpoint), HttpMethod.Get)
        {
            if (string.IsNullOrEmpty(siteId))
            {
                throw new System.ArgumentException("A site ID must be provided", nameof(siteId));
            }
        }
    }
}
