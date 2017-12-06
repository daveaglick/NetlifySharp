namespace NetlifySharp.Models
{
    public partial class SiteSetup : Model
    {
        // Not in Open API specification (https://github.com/netlify/open-api/issues/47)
        public ProcessingSettings ProcessingSettings { get; set; }
    }
}
