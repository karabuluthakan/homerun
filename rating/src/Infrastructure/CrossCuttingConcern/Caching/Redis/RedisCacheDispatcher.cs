using Core.Extensions;
using Domain.CrossCuttingConcern.Caching;
using StackExchange.Redis;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Infrastructure.CrossCuttingConcern.Caching.Redis;

public sealed class RedisCacheDispatcher : ICacheDispatcher, IDisposable
{
    private ConnectionMultiplexer _connection;
    private IDatabase _database;
    private readonly object _lockObject;
    private readonly string _connectionString;
    private readonly SemaphoreSlim _connectionLock;

    public RedisCacheDispatcher(string connectionString)
    {
        _connectionString = connectionString;
        _lockObject = new object();
        _connectionLock = new SemaphoreSlim(1, 1);
    }

    public bool Set(string key, object data, TimeSpan? expiry = null)
    {
        Connect();
        lock (_lockObject)
        {
            return _database.StringSet(key, data.ToJson(), expiry, When.Always);
        }
    }

    public async ValueTask<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        await ConnectAsync(cancellationToken);
        var data = await _database.StringGetAsync(key);
        return data.HasValue
            ? data.ToString().FromJson<T>()
            : default;
    }

    public async ValueTask<bool> IsExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        await ConnectAsync(cancellationToken);
        return await _database.KeyExistsAsync(key);
    }

    public async ValueTask<bool> RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await ConnectAsync(cancellationToken);
        return await _database.KeyDeleteAsync(key);
    }

    public async ValueTask RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default)
    {
        await ConnectAsync(cancellationToken);
        var keysToRemove = GetRemoveKeys(pattern);
        if (keysToRemove.Any()) await _database.KeyDeleteAsync(keysToRemove.ToArray());
    }

    public void Dispose()
    {
        Dispose(true);
    }

    private void Dispose(bool dispose)
    {
        if (dispose) _connection?.Dispose();
    }

    private List<RedisKey> GetRemoveKeys(string pattern)
    {
        var endPoint = _connection.GetEndPoints().FirstOrDefault();
        return _connection.GetServer(endPoint).Keys(pattern: $"*{pattern}*").ToList();
    }

    private void Connect()
    {
        if (_database is not null) return;

        _connectionLock.Wait();

        try
        {
            _connection = ConnectionMultiplexer.Connect(_connectionString);
            _database = _connection.GetDatabase();
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    private async Task ConnectAsync(CancellationToken token = default)
    {
        if (_database is not null) return;

        token.ThrowIfCancellationRequested();
        await _connectionLock.WaitAsync(token);

        try
        {
            _connection = await ConnectionMultiplexer.ConnectAsync(_connectionString);
            _database = _connection.GetDatabase();
        }
        finally
        {
            _connectionLock.Release();
        }
    }
}