using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.CSharp.Models;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp.Models;

namespace NetlifySharp.Generator
{
    public class CustomTemplateFactory : ITemplateFactory
    {
        private static readonly PropertyInfo HashObjectProperty = typeof(ITemplateFactory)
            .Assembly
            .GetTypes()
            .First(x => x.Name == "LiquidProxyHash")
            .GetTypeInfo()
            .GetProperty("Object");

        private readonly ITemplateFactory _defaultFactory;

        private readonly CSharpClientGeneratorSettings _clientSettings;

        private CSharpClientTemplateModel _clientTemplateModel = null;

        public CustomTemplateFactory(ITemplateFactory defaultFactory, CSharpClientGeneratorSettings clientSettings)
        {
            _defaultFactory = defaultFactory;
            _clientSettings = clientSettings;
        }

        public ITemplate CreateTemplate(string language, string templateName, object model)
        {
            ITemplate template = _defaultFactory.CreateTemplate(language, templateName, model);
            if (templateName.StartsWith("Client.Class.Body"))
            {
                // This gets called before the Class.Body template so use it to get the client template model
                _clientTemplateModel = (CSharpClientTemplateModel)HashObjectProperty.GetValue(model);
            }
            if (templateName.StartsWith("Class.Body"))
            {
                // Get all the operations so we can create proxies in the model objects
                ClassTemplateModel classTemplateModel = (ClassTemplateModel)HashObjectProperty.GetValue(model);
                return new ClassBodyTemplate(classTemplateModel, _clientTemplateModel);
            }
            if (templateName == "Class.Inheritance")
            {
                // Add IClientModel to the interface list
                return new ClassInheritanceTemplate(template);
            }
            if (templateName == "Class.Constructor")
            {
                // Changes the constructor to take a NetlifyClient
                ClassTemplateModel classTemplateModel = (ClassTemplateModel)HashObjectProperty.GetValue(model);
                return new ClassConstructorTemplate(classTemplateModel, _clientSettings);
            }
            return template;
        }
    }
}
