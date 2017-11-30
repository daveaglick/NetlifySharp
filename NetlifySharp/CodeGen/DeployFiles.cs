
using System;

namespace NetlifySharp.Models
{
    public partial class DeployFiles : Model
    {
        public bool? Async { get; private set; }
        public bool? Draft { get; private set; }
        public DeployFilesFiles Files { get; private set; }
        public DeployFilesFunctions Functions { get; private set; }
    }
}
