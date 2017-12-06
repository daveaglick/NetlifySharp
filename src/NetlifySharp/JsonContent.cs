using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NetlifySharp
{
    // From https://stackoverflow.com/a/25336147
    public class JsonContent : HttpContent
    {
        private readonly NetlifyClient _client;
        private readonly object _serializationTarget;

        public JsonContent(NetlifyClient client, object serializationTarget)
        {
            _client = client;
            _serializationTarget = serializationTarget;
            Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            await Task.Run(() =>
            {
                if (_serializationTarget == null)
                {
                    return;
                }

                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8, 1024, true))
                {
                    using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
                    {
                        _client.Serializer.Serialize(jsonWriter, _serializationTarget);
                    }
                }
            });
        }

        protected override bool TryComputeLength(out long length)
        {
            // We don't know, can't be computed up-front
            length = -1;
            return false;
        }
    }
}
