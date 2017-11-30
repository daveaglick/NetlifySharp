
using System;

namespace NetlifySharp.Models
{
    public partial class Form : Model
    {
        public DateTime? CreatedAt { get; private set; }
        public FormFields[] Fields { get; private set; }
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string[] Paths { get; private set; }
        public string SiteId { get; private set; }
        public int? SubmissionCount { get; private set; }
    }
}
