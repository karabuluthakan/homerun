using System.Reflection;
using Common.DependencyResolvers;
using Domain.CrossCuttingConcern.Caching;
using Domain.Repository; 
using Infrastructure.CrossCuttingConcern.Caching.Redis;
using Infrastructure.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Assemblies

var programAssembly = typeof(Program).Assembly;
var domainAssembly = AppDomain.CurrentDomain.Load("Domain");
var infrastructureAssembly = AppDomain.CurrentDomain.Load("Infrastructure"); 
var assemblies = new[] { programAssembly, domainAssembly, infrastructureAssembly };

#endregion

// Add services to the container.
builder.Services.AddDefaultDependencies();
builder.Services.AddFluentValidationDependency(assemblies);

var databaseConnectionString = builder.Configuration.GetConnectionString("DatabaseConnection");
if (string.IsNullOrWhiteSpace(databaseConnectionString))
{
    throw new ArgumentNullException(nameof(databaseConnectionString));
}

builder.Services.AddDbContextFactory<ChallengeDbContext>(options =>
{
    options.UseNpgsql(databaseConnectionString, b => b.MigrationsAssembly("Infrastructure"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

var cacheConnectionString = builder.Configuration.GetConnectionString("CacheConnection");
if (string.IsNullOrWhiteSpace(cacheConnectionString))
{
    throw new ArgumentNullException(nameof(cacheConnectionString));
}

builder.Services.AddScoped<ICacheDispatcher>(_ => new RedisCacheDispatcher(cacheConnectionString));

builder.Services
    .AddHealthCheckDependency()
    .AddNpgSql(databaseConnectionString)
    .AddRedis(cacheConnectionString);
var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
builder.Services.AddSwaggerDependency(xmlPath);

builder.Services.AddScoped<IRatingRepository, RatingEfCoreRepository>(); 

var app = builder.Build();
app.UseDefaultDependencies();

using (var scope = app.Services.CreateScope())
{
    using (var dbContext = scope.ServiceProvider.GetRequiredService<ChallengeDbContext>())
    {
        try
        {
            dbContext.Database.Migrate();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}

app.Run();

namespace Api
{
    public partial class Program
    {
    }
}