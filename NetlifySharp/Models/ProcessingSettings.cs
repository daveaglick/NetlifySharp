namespace NetlifySharp.Models
{
    // Not in Open API definition (https://github.com/netlify/open-api/issues/47)
    public partial class ProcessingSettings : Model
    {
        public CssProcessingSettings Css { get; set; }
        public JsProcessingSettings Js { get; set; }
        public ImagesProcessingSettings Images { get; set; }
        public HtmlProcessingSettings Html { get; set; }
        public bool Skip { get; set; }
    }
}
