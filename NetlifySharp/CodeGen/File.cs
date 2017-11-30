
using System;

namespace NetlifySharp.Models
{
    public partial class File : Model
    {
        public string Id { get; private set; }
        public string MimeType { get; private set; }
        public string Path { get; private set; }
        public string Sha { get; private set; }
        public int? Size { get; private set; }
    }
}
