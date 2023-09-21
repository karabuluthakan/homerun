using HealthChecks.UI.Client;
using HealthChecks.UI.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Common.DependencyResolvers;

public static class WebApplicationExtensions
{
    public static void UseDefaultDependencies(this WebApplication app)
    {
        app.UseCors("AllowAllCorsPolicy").UseForwardedHeaders();
        app.UseRouting();
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.UseHealthChecksUI(delegate(Options options)
        {
            //default value is “/healthchecks-ui”
            options.UIPath = "/health-ui";
        });
        
        app.UseSwagger();
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                var groupName = description.GroupName;
                options.SwaggerEndpoint($"/swagger/{groupName}/swagger.json",
                    $"Challenge Api {groupName.ToUpperInvariant()}");
            }
        
            options.DefaultModelExpandDepth(2);
            options.DefaultModelsExpandDepth(-1);
            options.DefaultModelRendering(ModelRendering.Model);
            options.DisplayOperationId();
            options.DisplayRequestDuration();
            options.DocExpansion(DocExpansion.List);
            options.EnableDeepLinking();
            options.EnableFilter();
            options.MaxDisplayedTags(5);
            options.ShowExtensions();
            options.ShowCommonExtensions();
            options.EnableValidator();
        });
        
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        
        app.UseReDoc(options =>
        {
            options.RoutePrefix = "documentation";
            foreach (var description in provider.ApiVersionDescriptions)
            {
                var groupName = description.GroupName;
                options.SpecUrl = $"/swagger/{groupName}/swagger.json";
                options.DocumentTitle = $"Challenge Api Documentation [{env}] {description.ApiVersion}";
            }
        
            options.EnableUntrustedSpec();
            options.ScrollYOffset(10);
            options.HideHostname();
            options.HideDownloadButton();
            options.RequiredPropsFirst();
            options.NoAutoAuth();
            options.PathInMiddlePanel();
            options.HideLoading();
            options.NativeScrollbars();
            options.DisableSearch();
            options.OnlyRequiredInSamples();
            options.SortPropsAlphabetically();
        });
        
        app.UseHttpsRedirection();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecksUI();
        });
    }
}