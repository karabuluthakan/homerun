using System.Net.Mime;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Common.Swagger.Filters;
using Common.Swagger.Options;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Swashbuckle.AspNetCore.Filters;

namespace Common.DependencyResolvers;

public static class ServiceCollectionExtensions
{
    private const string ApiVersionName = "X-Api-Version";

    public static void AddDefaultDependencies(this IServiceCollection services)
    {
        services.AddControllerDependency();
        services.AddCorsDependency();
        services.AddApiVersionDependency();
        services.AddSwaggerDependency();
    }

    private static void AddControllerDependency(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add(new ProducesAttribute(MediaTypeNames.Application.Json));
            options.Filters.Add(new ConsumesAttribute(MediaTypeNames.Application.Json));
        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }).AddMvcOptions(options =>
        {
            options.ModelMetadataDetailsProviders.Clear();
            options.ModelValidatorProviders.Clear();
        });
    }

    public static void AddFluentValidationDependency(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.AddValidatorsFromAssemblies(assemblies)
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddFluentValidationRulesToSwagger();
    }

    private static void AddCorsDependency(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllCorsPolicy", policy => policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetPreflightMaxAge(TimeSpan.FromSeconds(86400))
                .WithExposedHeaders("WWW-Authenticate"));
        });
    }

    private static void AddApiVersionDependency(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new QueryStringApiVersionReader(ApiVersionName),
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader(ApiVersionName),
                new MediaTypeApiVersionReader(ApiVersionName));
        });

        services.AddVersionedApiExplorer(opt =>
        {
            opt.GroupNameFormat = "'v'VVV";
            opt.SubstituteApiVersionInUrl = true;
        });

        services.TryAddSingleton<IApiDescriptionGroupCollectionProvider, ApiDescriptionGroupCollectionProvider>();
        services.TryAddEnumerable(ServiceDescriptor
            .Transient<IApiDescriptionProvider, DefaultApiDescriptionProvider>());

        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
        services.AddEndpointsApiExplorer();
    }

    private static void AddSwaggerDependency(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerDefaultValuesFilter>();
                options.SchemaFilter<SwaggerAutoRestSchemaFilter>();
                options.DocumentFilter<SwaggerJsonPatchDocumentFilter>();
                options.EnableAnnotations(enableAnnotationsForInheritance: true,
                    enableAnnotationsForPolymorphism: true);
                options.ResolveConflictingActions(x => x.First());
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                options.ExampleFilters();
                options.UseAllOfForInheritance();
                options.SelectSubTypesUsing(baseType =>
                {
                    return Assembly.GetExecutingAssembly().GetTypes().Where(type => type.IsSubclassOf(baseType));
                });
                options.SelectDiscriminatorNameUsing((_) => "TypeName");
                options.SelectDiscriminatorValueUsing((subType) => subType.Name);
                options.OrderActionsBy(
                    (apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
                options.DocInclusionPredicate((_, _) => true);
            })
            .AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly())
            .ConfigureOptions<ConfigureSwaggerSettings>();
    }

    public static IHealthChecksBuilder AddHealthCheckDependency(this IServiceCollection services)
    {
        services.AddHealthChecksUI().AddInMemoryStorage();
        return services.AddHealthChecks();
    }
}