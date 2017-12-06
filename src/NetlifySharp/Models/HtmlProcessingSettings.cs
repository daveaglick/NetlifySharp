namespace NetlifySharp.Models
{
    // Not in Open API specification (https://github.com/netlify/open-api/issues/47)
    public partial class HtmlProcessingSettings : Model
    {
        public HtmlProcessingSettings(NetlifyClient client) : base(client)
        {
        }

        public bool? PrettyUrls { get; set; }
    }
}
