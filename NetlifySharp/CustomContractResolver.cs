using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NetlifySharp
{
    internal class CustomContractResolver : DefaultContractResolver
    {
        private readonly NetlifyClient _client;

        public CustomContractResolver(NetlifyClient client)
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

            if (propertyInfo?.PropertyType == typeof(NetlifyClient))
            {
                // Inject a value for the client
                property.DefaultValueHandling = DefaultValueHandling.Populate;
                property.DefaultValue = _client;
            }
            else
            {
                // Convert between snake case in JSON and pascal case in .NET
                property.PropertyName = new string(ToSnakeCase(property.PropertyName).ToArray());
            }

            return property;
        }
        
        public IEnumerable<char> ToSnakeCase(string str)
        {
            yield return char.ToLowerInvariant(str[0]);
            for(int c = 1; c < str.Length; c++)
            {
                if(char.IsUpper(str[c]))
                {
                    yield return '_';
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
