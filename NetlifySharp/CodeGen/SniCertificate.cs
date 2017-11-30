
using System;

namespace NetlifySharp.Models
{
    public partial class SniCertificate : Model
    {
        public DateTime? CreatedAt { get; private set; }
        public string[] Domains { get; private set; }
        public DateTime? ExpiresAt { get; private set; }
        public string State { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
    }
}
