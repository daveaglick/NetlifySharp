
using System;

namespace NetlifySharp.Models
{
    public partial class DeployKey : Model
    {
        public DateTime? CreatedAt { get; private set; }
        public string Id { get; private set; }
        public string PublicKey { get; private set; }
    }
}
