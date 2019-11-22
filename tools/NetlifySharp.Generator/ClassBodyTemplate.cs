using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.CSharp.Models;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp.Models;

namespace NetlifySharp.Generator
{
    public class ClassBodyTemplate : ITemplate
    {
        private static readonly ClientCall[] ClientCalls = new ClientCall[]
        {
            new ClientCall("Site", "string", "siteId", "Id", "GetSite"),
            new ClientCall("AccountMembership", "string", "accountId", "Id", "GetAccount"),
            new ClientCall("Hook", "string", "hookId", "Id", "GetHook"),
            new ClientCall("Deploy", "string", "deployId", "Id", "GetDeploy", "GetSiteDeploy", "UpdateSiteDeploy", "RestoreSiteDeploy"),
            new ClientCall("Form", "string", "formId", "Id"),
            new ClientCall("Build", "string", "buildId", "Id", "GetSiteBuild")
        };

        private readonly ClassTemplateModel _classTemplateModel;

        private readonly CSharpClientTemplateModel _clientTemplateModel;

        public ClassBodyTemplate(ClassTemplateModel classTemplateModel, CSharpClientTemplateModel clientTemplateModel)
        {
            _classTemplateModel = classTemplateModel ?? throw new ArgumentNullException(nameof(classTemplateModel));
            _clientTemplateModel = clientTemplateModel ?? throw new ArgumentNullException(nameof(clientTemplateModel));
        }

        public string Render()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("// Client methods");
            foreach (ClientCall clientCall in ClientCalls.Where(x => x.ClassName == _classTemplateModel.ClassName))
            {
                foreach (CSharpOperationModel operation in _clientTemplateModel.Operations
                    .Where(x => x.Parameters.Any(p => p.Type == clientCall.ParameterType && p.VariableName == clientCall.VariableName)
                        && !clientCall.ExcludedMethods.Contains(x.ActualOperationName)))
                {
                    BuildMethod(builder, clientCall, operation, false);
                    BuildMethod(builder, clientCall, operation, true);
                }
            }
            return builder.ToString();
        }

        private static void BuildMethod(StringBuilder builder, ClientCall clientCall, CSharpOperationModel operation, bool cancellationToken)
        {
            builder.Append($"public {operation.ResultType} {operation.ActualOperationName}Async(");
            BuildParameters(builder, clientCall, operation, cancellationToken, false);
            builder.Append($") => Client.{operation.ActualOperationName}Async(");
            BuildParameters(builder, clientCall, operation, cancellationToken, true);
            builder.AppendLine(");");
        }

        private static void BuildParameters(StringBuilder builder, ClientCall clientCall, CSharpOperationModel operation, bool cancellationToken, bool call)
        {
            bool comma = false;

            // Build the parameters
            foreach (CSharpParameterModel parameter in operation.Parameters)
            {
                if (parameter.VariableName != clientCall.VariableName)
                {
                    if (comma)
                    {
                        builder.Append(", ");
                    }
                    if (!call)
                    {
                        builder.Append($"{parameter.Type} ");
                    }
                    builder.Append(parameter.VariableName);
                    comma = true;
                }
                else if (call)
                {
                    if (comma)
                    {
                        builder.Append(", ");
                    }
                    builder.Append(clientCall.ModelProperty);
                    comma = true;
                }
            }

            // Add the cancellation token
            if (cancellationToken)
            {
                if (comma)
                {
                    builder.Append(", ");
                }
                if (!call)
                {
                    builder.Append("System.Threading.CancellationToken ");
                }
                builder.Append("cancellationToken");
            }
        }

        public class ClientCall
        {
            public ClientCall(string className, string parameterType, string variableName, string modelProperty, params string[] excludedMethods)
            {
                ClassName = className;
                ParameterType = parameterType;
                VariableName = variableName;
                ModelProperty = modelProperty;
                ExcludedMethods = excludedMethods;
            }

            public string ClassName { get; }

            public string ParameterType { get; }

            public string VariableName { get; }

            public string ModelProperty { get; }

            public string[] ExcludedMethods { get; }
        }
    }
}
