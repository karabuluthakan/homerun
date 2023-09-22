using DotNet.Testcontainers.Builders;
using Infrastructure.DataAccess.EntityFramework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace Api.Test.TestContract;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public PostgreSqlContainer DbContainer;
    public RedisContainer CacheContainer;
    public RabbitMqContainer QueueContainer;

    public IntegrationTestWebAppFactory()
    {
        DbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithName("challenge_postgres_test")
            .WithEnvironment("POSTGRES_USER", "postgres")
            .WithEnvironment("POSTGRES_PASSWORD", "postgres")
            .WithEnvironment("POSTGRES_DB", "postgres")
            .WithCleanUp(true)
            .Build();

        CacheContainer = new RedisBuilder()
            .WithImage("redis")
            .WithName("challenge_redis_test")
            .WithCleanUp(true)
            .Build();

        QueueContainer = new RabbitMqBuilder()
            .WithImage("rabbitmq:3-management")
            .WithName("challenge_rabbitmq_test")
            .WithCleanUp(true)
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var dbContextDescriptor = services
                .SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<ChallengeDbContext>));

            if (dbContextDescriptor is not null) services.Remove(dbContextDescriptor);

            var databaseConnectionString = DbContainer.GetConnectionString();
            services.AddDbContextFactory<ChallengeDbContext>(options =>
            {
                options.UseNpgsql(databaseConnectionString, b => b.MigrationsAssembly("Infrastructure"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
        });
    }

    public async Task InitializeAsync()
    {
        var db = DbContainer.StartAsync();
        var cache = CacheContainer.StartAsync();
        var queue = QueueContainer.StartAsync();
        await Task.WhenAll(db, cache, queue);
        await db;
        await cache;
        await queue;
    }

    public new async Task DisposeAsync()
    {
        var db = DbContainer.StopAsync();
        var cache = CacheContainer.StopAsync();
        var queue = QueueContainer.StopAsync();
        await Task.WhenAll(db, cache, queue);
        await db;
        await cache;
        await queue;

        var dbDispose = DbContainer.DisposeAsync().AsTask();
        var cacheDispose = CacheContainer.DisposeAsync().AsTask();
        var queueDispose = QueueContainer.DisposeAsync().AsTask();
        await Task.WhenAll(dbDispose, cacheDispose, queueDispose);
        await dbDispose;
        await cacheDispose;
        await queueDispose;
    }
}