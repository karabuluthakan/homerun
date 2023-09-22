using System.Reflection;
using Core.Attributes;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Common.Swagger.Filters;

public class SwaggerIgnoreEnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            var enumOpenApiStrings = new List<IOpenApiAny>();
            foreach (var enumValue in Enum.GetValues(context.Type))
            {
                var member = context.Type.GetMember(enumValue.ToString())[0];
                if (!member.GetCustomAttributes<SwaggerIgnoreEnumAttribute>().Any())
                {
                    enumOpenApiStrings.Add(new OpenApiString(enumValue.ToString()));
                }
            }
            schema.Enum = enumOpenApiStrings;
        }
    }
}