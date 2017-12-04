using System.Net.Http;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Linq;

namespace NetlifySharp.Operations
{
    public abstract class Operation<TThis> : OperationBase<TThis>
        where TThis : Operation<TThis>
    {
        protected Operation(NetlifyClient client, Endpoint endpoint, HttpMethod method)
            : base(client, endpoint, method)
        {
        }

        public async Task SendAsync(CancellationToken cancellationToken = default(CancellationToken)) =>
            await GetResponseAsync(cancellationToken);
    }
}