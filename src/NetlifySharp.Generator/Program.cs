using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NJsonSchema;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.CSharp;
using NJsonSchema.CodeGeneration.CSharp.Models;
using NSwag;
using NSwag.CodeGeneration;
using NSwag.CodeGeneration.CSharp;

namespace NetlifySharp.Generator
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Read the Open API YAML specification
            OpenApiDocument document = await OpenApiYamlDocument.FromUrlAsync("https://raw.githubusercontent.com/netlify/open-api/master/swagger.yml");

            // Generate the code
            CSharpClientGeneratorSettings clientSettings = new CSharpClientGeneratorSettings
            {
                ClassName = nameof(NetlifyClient),
                CSharpGeneratorSettings =
                {
                    Namespace = nameof(NetlifySharp),
                    PropertyNameGenerator = new PascalCasePropertyNameGenerator()
                },
                ParameterNameGenerator = new CamelCaseParameterNameGenerator(),
                ExceptionClass = nameof(NetlifyException),
                InjectHttpClient = false,
                UseHttpClientCreationMethod = true
            };
            clientSettings.CSharpGeneratorSettings.TemplateFactory = new CustomTemplateFactory(
                clientSettings.CSharpGeneratorSettings.TemplateFactory,
                clientSettings);
            CSharpClientGenerator generator = new CSharpClientGenerator(document, clientSettings);
            string code = generator.GenerateFile();

            // Make the constructor private so we can write our own
            code = code.Replace($"public {clientSettings.ClassName}(", $"private {clientSettings.ClassName}(");

            // Write to a file in the client project
            System.IO.File.WriteAllText($"../NetlifySharp/{clientSettings.ClassName}.Generated.cs", code);
        }
    }
}
