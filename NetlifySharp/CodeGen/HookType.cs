
using System;

namespace NetlifySharp.Models
{
    public partial class HookType : Model
    {
        public string[] Events { get; private set; }
        public HookTypeFields[] Fields { get; private set; }
        public string Name { get; private set; }
    }
}
