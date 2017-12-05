#r "..\CodeGenLibs\Newtonsoft.Json.dll"

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

// Clean previous generations
if (Directory.Exists("CodeGen"))
{
    Directory.Delete("CodeGen", true);
}

// Get and parse the Open API definition
string swagger = File.ReadAllText("swagger.json");
JObject root = JObject.Parse(swagger);

// Iterate the definitions
JObject definitions = (JObject)root["definitions"];
foreach (JProperty definition in definitions.Properties())
{
    WriteObject(ToCamelCase(definition.Name), (JObject)definition.Value, definitions);
}

// Helper methods

public static Dictionary<string, string> ClassNameMappings = new Dictionary<string, string>
{
    { "AccessToken", "AccessTokenData" }
};

public static Dictionary<string, string> TypeMappings = new Dictionary<string, string>
{
    { "integer", "int?" },
    { "boolean", "bool?" },
    { "number", "int?" }
};

public static Dictionary<string, string> FormatMappings = new Dictionary<string, string>
{
    { "string", "string" },
    { "dateTime", "DateTime?" },
    { "int32", "int?" }
};

// Some classes need to be renamed due to duplicate named properties
public string GetClassName(String name) =>
    ClassNameMappings.ContainsKey(name) ? ClassNameMappings[name] : name;


public void WriteObject(string name, JObject definition, JObject definitions)
{
    string className = GetClassName(name);
    using (TextWriter writer = Output[$@"CodeGen\{className}.cs"])
    {
        writer.Write($@"
using System;

namespace NetlifySharp.Models
{{
    public partial class {className} : Model
    {{
        public {className}(NetlifyClient client) : base(client)
        {{
        }}");
        WriteProperties(name, writer, definition, definitions, true);
        writer.Write($@"
    }}
}}
");
    }
}

public void WriteProperties(string containingName, TextWriter writer, JObject definition, JObject definitions, bool writeObjects)
{
    // Handle allOf
    JProperty allOf = definition.Property("allOf");
    if(allOf != null)
    {
        foreach(JObject allOfItem in (JArray)allOf.Value)
        {
            JProperty reference = allOfItem.Property("$ref");
            if (reference != null)
            {
                string referenceType = reference.Value.ToString().Replace("#/definitions/", string.Empty);
                JProperty referenceDefinition = definitions.Property(referenceType);
                if (referenceDefinition == null)
                {
                    throw new Exception($"Unknown reference type {referenceType}");
                }
                WriteProperties(ToCamelCase(referenceType), writer, (JObject)referenceDefinition.Value, definitions, false);
            }
            else if(allOfItem.Property("properties") != null)
            {
                WriteProperties(containingName, writer, allOfItem, definitions, writeObjects);
            }
            else
            {
                throw new Exception($"Unknown composition item {allOfItem.ToString()}");
            }
        }
        return;
    }

    // Iterate properties
    JObject properties = definition.Property("properties")?.Value as JObject;
    if (properties != null)
    {
        foreach (JProperty property in properties.Properties())
        {
            string propertyName = containingName + ToCamelCase(property.Name);
            writer.Write($@"
        public { GetPropertyType(propertyName, (JObject)property.Value, writeObjects) } { ToCamelCase(property.Name) } {{ get; set; }}");
        }
    }
}

public string GetPropertyType(string name, JObject property, bool writeObjects)
{
    // Check if this references a different definition
    JProperty reference = property.Property("$ref");
    if(reference != null)
    {
        return GetClassName(
            ToCamelCase(
                reference.Value.ToString().Replace("#/definitions/", string.Empty)));
    }

    // Process the type
    string type = property.Property("type")?.Value.ToString() ?? "string";
    if (type == "object")
    {
        type = name;
        if (writeObjects)
        {
            WriteObject(name, property, definitions);
        }
    }
    else if (type == "array")
    {
        JObject items = (JObject)property.Property("items")?.Value;
        if(items == null)
        {
            throw new Exception("Unexpected array without items property");
        }
        type = $"{GetPropertyType(name, items, writeObjects)}[]";
    }
    else if (type == "string")
    {
        string format = property.Property("format")?.Value.ToString();
        if(format != null)
        {
            if(!FormatMappings.TryGetValue(format, out type))
            {
                throw new Exception($"Unknown property format {format}");
            }
        }
    }
    else
    {
        string originalType = type;
        if(!TypeMappings.TryGetValue(type, out type))
        {
            throw new Exception($"Unknown property type {originalType}");
        }
    }
    return type;
}

public string ToCamelCase(string str) => new string(ToCamelCaseChars(str).ToArray());

public IEnumerable<char> ToCamelCaseChars(string str)
{
    yield return char.ToUpperInvariant(str[0]);
    for(int c = 1; c < str.Length; c++)
    {
        if(str[c] == '_')
        {
            c++;
            if(c < str.Length)
            {
                yield return char.ToUpperInvariant(str[c]);
            }
        }
        else
        {
            yield return str[c];
        }
    }
}