using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NJsonSchema;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.CSharp;
using NSwag;
using NSwag.CodeGeneration;
using NSwag.CodeGeneration.CSharp;

namespace NetlifySharp.Generator
{
    public class CamelCaseParameterNameGenerator : IParameterNameGenerator
    {
        public string Generate(OpenApiParameter parameter, IEnumerable<OpenApiParameter> allParameters) =>
            string.Concat(
                parameter.Name
                    .Split(new char[] { '_', '-' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select((x, i) => i == 0 ? x : (x.Length > 1 ? char.ToUpperInvariant(x[0]) + x.Substring(1) : x.ToUpperInvariant())));
    }
}
