namespace NetlifySharp.Models
{
    // Not in Open API specification (https://github.com/netlify/open-api/issues/47)
    public partial class ImagesProcessingSettings : Model
    {
        public ImagesProcessingSettings(NetlifyClient client) : base(client)
        {
        }

        public bool? Optimize { get; set; }
    }
}
