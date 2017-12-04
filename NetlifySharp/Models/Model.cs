using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace NetlifySharp.Models
{
    public abstract class Model
    {
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; private set; }
    }
}
