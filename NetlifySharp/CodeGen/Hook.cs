
using System;

namespace NetlifySharp.Models
{
    public partial class Hook : Model
    {
        public DateTime? CreatedAt { get; private set; }
        public HookData Data { get; private set; }
        public bool? Disabled { get; private set; }
        public string Event { get; private set; }
        public string Id { get; private set; }
        public string SiteId { get; private set; }
        public string Type { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
    }
}
