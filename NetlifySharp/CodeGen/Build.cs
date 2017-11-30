
using System;

namespace NetlifySharp.Models
{
    public partial class Build : Model
    {
        public DateTime? CreatedAt { get; private set; }
        public string DeployId { get; private set; }
        public bool? Done { get; private set; }
        public string Error { get; private set; }
        public string Id { get; private set; }
        public string Sha { get; private set; }
    }
}
