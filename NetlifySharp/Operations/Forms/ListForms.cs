using System.Net.Http;
using System.Runtime.CompilerServices;
using NetlifySharp.Models;

namespace NetlifySharp.Operations.Forms
{
    public class ListForms : PagedOperation<Form[], ListForms>
    {
        internal ListForms(NetlifyClient client)
            : base(client, NetlifyClient.FormsEndpoint, HttpMethod.Get)
        {
        }
    }
}
