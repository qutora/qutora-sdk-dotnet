using Qutora.SDK.Interfaces;

namespace Qutora.SDK.Cache.Providers;

/// <summary>
/// Null cache provider implementation that performs no caching operations
/// </summary>
public class NullCacheProvider : ICacheProvider
{
    /// <inheritdoc />
    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(default(T));
    }

    /// <inheritdoc />
    public Task SetAsync<T>(string key, T value, TimeSpan duration, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task ClearAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
} 