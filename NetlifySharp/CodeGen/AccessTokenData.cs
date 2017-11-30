
using System;

namespace NetlifySharp.Models
{
    public partial class AccessTokenData : Model
    {
        public string AccessToken { get; private set; }
        public DateTime? CreatedAt { get; private set; }
        public string Id { get; private set; }
        public string UserEmail { get; private set; }
        public string UserId { get; private set; }
    }
}
