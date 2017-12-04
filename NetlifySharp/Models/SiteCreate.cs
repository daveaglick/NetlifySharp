namespace NetlifySharp.Models
{
    // Not in Open API specification (https://github.com/netlify/open-api/issues/48)
    public partial class SiteCreate : Model
    {
        // Not in Open API specification (https://github.com/netlify/open-api/issues/47)
        public ProcessingSettings ProcessingSettings { get; set; }

        public string Name { get; set; }
        public string CustomDomain { get; set; }
        public string Password { get; set; }
        public bool? ForceSsl { get; set; }
        public RepoSetup Repo { get; set; }
    }
}
