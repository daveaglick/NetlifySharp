using NetlifySharp.Operations.Sites;
using System;
using System.IO;
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
        public bool? Premium { get; set; }
        public bool? Claimed { get; set; }

        // Operations
        public DeleteSite DeleteSite() => new DeleteSite(Client, Id);
        public UpdateSite UpdateSite(SiteSetup siteSetup) => new UpdateSite(Client, Id, siteSetup);
        public UpdateSite UpdateSite(Stream zipStream) => new UpdateSite(Client, Id, zipStream);
        public UpdateSite UpdateSite(string directory) => new UpdateSite(Client, Id, directory);
        public ListSiteForms ListSiteForms() => new ListSiteForms(Client, Id);
    }
}
