using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Common.Swagger.Filters;

public sealed class SwaggerJsonPatchDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        foreach (var (key, _) in swaggerDoc.Components.Schemas)
            if (key.StartsWith("Operation") || key.StartsWith("JsonPatchDocument"))
                swaggerDoc.Components.Schemas.Remove(key);

        swaggerDoc.Components.Schemas.Add("Operation", new OpenApiSchema
        {
            Type = "object",
            Properties = new Dictionary<string, OpenApiSchema>
            {
                { "op", new OpenApiSchema { Type = "string" } },
                { "value", new OpenApiSchema { Type = "object", Nullable = true } },
                { "path", new OpenApiSchema { Type = "string" } }
            }
        });

        swaggerDoc.Components.Schemas.Add("JsonPatchDocument", new OpenApiSchema
        {
            Type = "array",
            Items = new OpenApiSchema
            {
                Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "Operation" }
            },
            Description = "Array of operations to perform"
        });


        const string patchJson = "application/json-patch+json";
        foreach (var path in swaggerDoc.Paths.SelectMany(x => x.Value.Operations)
                     .Where(x => x.Key.Equals(OperationType.Patch)))
        {
            foreach (var item in path.Value.RequestBody.Content.Where(x => !x.Key.Equals(patchJson)))
                path.Value.RequestBody.Content.Remove(item.Key);

            var response = path.Value.RequestBody.Content.Single(x => x.Key.Equals(patchJson));
            response.Value.Schema = new OpenApiSchema
            {
                Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "JsonPatchDocument" }
            };
        }
    }
}