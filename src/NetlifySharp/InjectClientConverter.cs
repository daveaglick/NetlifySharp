using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;

namespace NetlifySharp
{
    internal class InjectClientConverter : CustomCreationConverter<object>
    {
        private readonly NetlifyClient _client;

        public InjectClientConverter(NetlifyClient client)
        {
            _client = client;
        }

        public override object Create(Type objectType) => Activator.CreateInstance(objectType, _client);
    }
}