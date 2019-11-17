using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NetlifySharp
{
    internal class ClientModelCreationConverter : CustomCreationConverter<object>
    {
        private readonly NetlifyClient _client;

        public ClientModelCreationConverter(NetlifyClient client)
        {
            _client = client;
        }

        public override bool CanConvert(Type objectType)
        {
            TypeInfo typeInfo = objectType.GetTypeInfo();
            return typeof(IClientModel).GetTypeInfo().IsAssignableFrom(typeInfo)
                && typeInfo.DeclaredConstructors.Any(x =>
                {
                    ParameterInfo[] parameters = x.GetParameters();
                    return x.IsPublic && parameters.Length == 1 && parameters[0].ParameterType == typeof(NetlifyClient);
                });
        }

        public override object Create(Type objectType) => Activator.CreateInstance(objectType, _client);
    }
}