using System;
using NJsonSchema.CodeGeneration;
using NSwag.CodeGeneration.CSharp;

namespace NetlifySharp.Generator
{
    public class ClassInheritanceTemplate : ITemplate
    {
        private readonly ITemplate _template;

        public ClassInheritanceTemplate(ITemplate template)
        {
            _template = template ?? throw new ArgumentNullException(nameof(template));
        }

        public string Render()
        {
            string render = _template.Render();
            render += string.IsNullOrEmpty(render) ? ": IClientModel" : ", IClientModel";
            return render;
        }
    }
}
