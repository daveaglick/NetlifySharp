using System.Net.Http;

namespace NetlifySharp.Operations.Sites
{
    public class DeleteSite : VoidOperation<DeleteSite>
    {
        internal DeleteSite(NetlifyClient client, string siteId)
            : base(client, NetlifyClient.SitesEndpoint.Append(siteId), HttpMethod.Delete)
        {
            if (string.IsNullOrEmpty(siteId))
            {
                throw new System.ArgumentException("A site ID must be provided", nameof(siteId));
            }
        }
    }
}
