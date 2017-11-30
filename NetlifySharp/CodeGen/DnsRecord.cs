
using System;

namespace NetlifySharp.Models
{
    public partial class DnsRecord : Model
    {
        public string Hostname { get; private set; }
        public string Id { get; private set; }
        public int? Priority { get; private set; }
        public int? Ttl { get; private set; }
        public string Type { get; private set; }
        public string Value { get; private set; }
    }
}
