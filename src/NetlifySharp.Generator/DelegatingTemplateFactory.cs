using NJsonSchema.CodeGeneration;
using NSwag.CodeGeneration.CSharp;

namespace NetlifySharp.Generator
{
    public class DelegatingTemplateFactory : ITemplateFactory
    {
        private readonly ITemplateFactory _defaultFactory;
        private readonly CSharpClientGeneratorSettings _clientSettings;

        public DelegatingTemplateFactory(ITemplateFactory defaultFactory, CSharpClientGeneratorSettings clientSettings)
        {
            _defaultFactory = defaultFactory;
            _clientSettings = clientSettings;
        }

        public ITemplate CreateTemplate(string language, string templateName, object model)
        {
            ITemplate template = _defaultFactory.CreateTemplate(language, templateName, model);
            if (templateName == "Class.Constructor")
            {
                return new ClassConstructorTemplate((DotLiquid.Hash)model, _clientSettings);
            }
            return template;
        }
    }
}
