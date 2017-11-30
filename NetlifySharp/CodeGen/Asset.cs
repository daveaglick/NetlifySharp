
using System;

namespace NetlifySharp.Models
{
    public partial class Asset : Model
    {
        public string ContentType { get; private set; }
        public DateTime? CreatedAt { get; private set; }
        public string CreatorId { get; private set; }
        public string Id { get; private set; }
        public string Key { get; private set; }
        public string Name { get; private set; }
        public string SiteId { get; private set; }
        public int? Size { get; private set; }
        public string State { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public string Url { get; private set; }
        public string Visibility { get; private set; }
    }
}
