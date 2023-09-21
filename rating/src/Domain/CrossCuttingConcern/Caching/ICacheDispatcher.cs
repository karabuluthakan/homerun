namespace Domain.CrossCuttingConcern.Caching;

public interface ICacheDispatcher
{
    bool Set(string key, object data, TimeSpan? expiry = null);
    ValueTask<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);
    ValueTask<bool> IsExistsAsync(string key, CancellationToken cancellationToken = default);
    ValueTask<bool> RemoveAsync(string key, CancellationToken cancellationToken = default);
    ValueTask RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default);
}