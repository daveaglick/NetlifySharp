
using System;

namespace NetlifySharp.Models
{
    public partial class Error : Model
    {
        public int? Code { get; private set; }
        public string Message { get; private set; }
    }
}
