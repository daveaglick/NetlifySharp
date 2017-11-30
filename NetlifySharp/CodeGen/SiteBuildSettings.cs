
using System;

namespace NetlifySharp.Models
{
    public partial class SiteBuildSettings : Model
    {
        public string[] AllowedBranches { get; private set; }
        public string Branch { get; private set; }
        public string Cmd { get; private set; }
        public string Dir { get; private set; }
    }
}
