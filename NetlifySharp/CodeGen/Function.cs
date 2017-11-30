
using System;

namespace NetlifySharp.Models
{
    public partial class Function : Model
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Sha { get; private set; }
    }
}
