using System;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.CSharp.Models;
using NSwag.CodeGeneration.CSharp;

namespace NetlifySharp.Generator
{
    public class ClassConstructorTemplate : ITemplate
    {
        private readonly ClassTemplateModel _classTemplateModel;
        private readonly CSharpClientGeneratorSettings _clientSettings;

        public ClassConstructorTemplate(ClassTemplateModel classTemplateModel, CSharpClientGeneratorSettings clientSettings)
        {
            _classTemplateModel = classTemplateModel ?? throw new ArgumentNullException(nameof(classTemplateModel));
            _clientSettings = clientSettings ?? throw new ArgumentNullException(nameof(clientSettings));
        }

        public string Render()
        {
            if (string.IsNullOrEmpty(_classTemplateModel.BaseClassName)
                || _classTemplateModel.BaseClassName.StartsWith("System."))
            {
                // No base class
                return @$"public {_classTemplateModel.ClassName}({_clientSettings.ClassName} client)
{{
    Client = client ?? throw new System.ArgumentNullException(nameof(client));
}}

[Newtonsoft.Json.JsonIgnore]
public {_clientSettings.ClassName} Client {{ get; internal set; }}
    
// Serializable properties";
            }

            // Base class
            return @$"public {_classTemplateModel.ClassName}({_clientSettings.ClassName} client) : base(client)
{{
}}
    
// Serializable properties";
        }
    }
}
