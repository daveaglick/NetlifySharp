
using System;

namespace NetlifySharp.Models
{
    public partial class RepoSetup : Model
    {
        public string[] AllowedBranches { get; private set; }
        public string Branch { get; private set; }
        public string Cmd { get; private set; }
        public string DeployKeyId { get; private set; }
        public string Dir { get; private set; }
        public int? Id { get; private set; }
        public bool? PrivateLogs { get; private set; }
        public string Provider { get; private set; }
        public bool? PublicRepo { get; private set; }
        public string Repo { get; private set; }
    }
}
