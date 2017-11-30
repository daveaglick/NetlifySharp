using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace NetlifySharp.Models
{
    public class Model
    {
        public NetlifyClient Client { get; internal set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; private set; }
    }
}
