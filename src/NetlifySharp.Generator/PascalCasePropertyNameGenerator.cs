using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NJsonSchema;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.CSharp;
using NSwag;
using NSwag.CodeGeneration.CSharp;

namespace NetlifySharp.Generator
{
    public class PascalCasePropertyNameGenerator : IPropertyNameGenerator
    {
        public string Generate(JsonSchemaProperty property) =>
            string.Concat(
                property.Name
                    .Split(new char[] { '_', '-' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Length > 1 ? char.ToUpperInvariant(x[0]) + x.Substring(1) : x.ToUpperInvariant()));
    }
}
