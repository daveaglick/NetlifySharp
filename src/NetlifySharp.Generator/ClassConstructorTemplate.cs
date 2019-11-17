using NJsonSchema.CodeGeneration;
using NSwag.CodeGeneration.CSharp;

namespace NetlifySharp.Generator
{
    public class ClassConstructorTemplate : ITemplate
    {
        private readonly DotLiquid.Hash _model;
        private readonly CSharpClientGeneratorSettings _clientSettings;

        public ClassConstructorTemplate(DotLiquid.Hash model, CSharpClientGeneratorSettings clientSettings)
        {
            _model = model;
            _clientSettings = clientSettings;
        }

        public string Render()
        {
            string className = _model.Get<string>("ClassName");
            string baseClassName = _model.Get<string>("BaseClassName");
            if (string.IsNullOrEmpty(baseClassName) || baseClassName.StartsWith("System."))
            {
                // No base class
                return @$"public {className}({_clientSettings.ClassName} client)
{{
    Client = client ?? throw new System.ArgumentNullException(nameof(client));
}}

[Newtonsoft.Json.JsonIgnore]
public {_clientSettings.ClassName} Client {{ get; internal set; }}
    
// Serializable properties";
            }

            // Base class
            return @$"public {className}({_clientSettings.ClassName} client) : base(client)
{{
}}
    
// Serializable properties";
        }
    }
}
