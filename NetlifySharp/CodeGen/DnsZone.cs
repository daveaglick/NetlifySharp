
using System;

namespace NetlifySharp.Models
{
    public partial class DnsZone : Model
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public DnsRecord[] Records { get; private set; }
    }
}
