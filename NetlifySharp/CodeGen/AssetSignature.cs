
using System;

namespace NetlifySharp.Models
{
    public partial class AssetSignature : Model
    {
        public Asset Asset { get; private set; }
        public AssetForm Form { get; private set; }
    }
}
