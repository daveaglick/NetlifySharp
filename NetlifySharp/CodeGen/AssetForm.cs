
using System;

namespace NetlifySharp.Models
{
    public partial class AssetForm : Model
    {
        public AssetFormFields Fields { get; private set; }
        public string Url { get; private set; }
    }
}
