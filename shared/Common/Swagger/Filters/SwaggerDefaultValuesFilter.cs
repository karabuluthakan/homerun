using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Common.Swagger.Filters;

public sealed class SwaggerDefaultValuesFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Deprecated |= context.ApiDescription.IsDeprecated();
        var parameters = operation.Parameters;
        if (parameters is null || !parameters.Any()) return;
        foreach (var parameter in parameters)
        {
            var description = context.ApiDescription.ParameterDescriptions
                .Where(x => x.Name == parameter.Name).First();

            if (string.IsNullOrWhiteSpace(parameter.Description))
                parameter.Description = description.ModelMetadata?.Description;

            if (parameter.Schema.Default is null && description.DefaultValue is not null)
                parameter.Schema.Default = new OpenApiString(description.DefaultValue.ToString());

            parameter.Required |= description.IsRequired;
        }
    }
}