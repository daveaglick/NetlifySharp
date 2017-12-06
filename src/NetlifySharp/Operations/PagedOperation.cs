using System.Net.Http;

namespace NetlifySharp.Operations
{
    public abstract class PagedOperation<TResponse, TThis> : ResponseOperation<TResponse, TThis>
        where TResponse : class
        where TThis : PagedOperation<TResponse, TThis>
    {
        protected PagedOperation(NetlifyClient client, Endpoint endpoint, HttpMethod method)
            : base(client, endpoint, method)
        {
        }

        public TThis WithPage(int page)
        {
            Query["page"] = page.ToString();
            return (TThis)this;
        }

        public TThis WithPerPage(int perPage)
        {
            Query["per_page"] = perPage.ToString();
            return (TThis)this;
        }
    }
}