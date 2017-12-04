using NetlifySharp.Operations.Sites;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NetlifySharp.Models
{
    public partial class Site : Model
    {
        // Not in Open API specification (https://github.com/netlify/open-api/issues/49)
        public PlanData PlanData { get; set; }
        
        // Not in Open API specification (https://github.com/netlify/open-api/issues/47)
        public ProcessingSettings ProcessingSettings { get; set; }

        // Not in Open API specification
        public string SiteId { get; set; }
        public bool Premium { get; set; }
        public bool Claimed { get; set; }

        // Operations
        public ListSiteForms ListSiteForms(NetlifyClient client) => new ListSiteForms(client, Id);
    }
}
