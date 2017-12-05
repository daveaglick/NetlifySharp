using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace NetlifySharp.Models
{
    public abstract class Model
    {
        protected Model(NetlifyClient client)
        {
            Client = client ?? throw new System.ArgumentNullException(nameof(client));
        }

        [JsonIgnore]
        public NetlifyClient Client { get; }

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; private set; }
    }
}
