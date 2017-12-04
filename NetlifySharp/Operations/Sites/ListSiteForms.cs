using System.Net.Http;
using System.Runtime.CompilerServices;
using NetlifySharp.Models;

namespace NetlifySharp.Operations.Sites
{
    public class ListSiteForms : Operation<Form[], ListSiteForms>
    {
        internal ListSiteForms(NetlifyClient client, string siteId)
            : base(
                  client,
                  NetlifyClient.SitesEndpoint.Append(siteId, nameof(siteId)).Append(NetlifyClient.FormsEndpoint),
                  HttpMethod.Get)
        {
        }
    }
}
