
using System;

namespace NetlifySharp.Models
{
    public partial class BuildLogMsg : Model
    {
        public bool? Error { get; private set; }
        public string Message { get; private set; }
    }
}
