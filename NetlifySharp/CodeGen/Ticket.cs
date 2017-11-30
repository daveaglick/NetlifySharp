
using System;

namespace NetlifySharp.Models
{
    public partial class Ticket : Model
    {
        public bool? Authorized { get; private set; }
        public string ClientId { get; private set; }
        public DateTime? CreatedAt { get; private set; }
        public string Id { get; private set; }
    }
}
