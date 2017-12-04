using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NetlifySharp
{
    internal class ClientContractResolver : DefaultContractResolver
    {
        private readonly NetlifyClient _client;

        public ClientContractResolver(NetlifyClient client)
        {
            _client = client;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            PropertyInfo propertyInfo = member as PropertyInfo;
            
            // Use a private setter if the property has one
            if (!property.Writable)
            {
                property.Writable = propertyInfo?.GetSetMethod(true) != null;
            }

            // Convert between snake case in JSON and pascal case in .NET
            property.PropertyName = ToSnakeCase(property.PropertyName);

            return property;
        }

        public static string ToSnakeCase(string str) => new string(ToSnakeCaseChars(str).ToArray());

        private static IEnumerable<char> ToSnakeCaseChars(string str)
        {
            yield return char.ToLowerInvariant(str[0]);
            for(int c = 1; c < str.Length; c++)
            {
                if(char.IsUpper(str[c]))
                {
                    if (str[c - 1] != '_')
                    {
                        yield return '_';
                    }
                    yield return char.ToLowerInvariant(str[c]);
                }
                else
                {
                    yield return str[c];
                }
            }
        }
    }
}
