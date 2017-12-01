using System.Net.Http;
using System.Runtime.CompilerServices;
using NetlifySharp.Models;

namespace NetlifySharp.Operations
{
    public class ListForms : Operation<Form[], ListForms>
    {
        internal ListForms(NetlifyClient client)
            : base(client, NetlifyClient.FormsEndpoint, HttpMethod.Get)
        {
        }
    }
}
