using Microsoft.Extensions.Caching.Distributed;
using Qutora.SDK.Interfaces;
using System.Text.Json;

namespace Qutora.SDK.Cache.Providers;

/// <summary>
/// Distributed cache provider implementation using IDistributedCache
/// </summary>
public class DistributedCacheProvider : ICacheProvider
{
    private readonly IDistributedCache _distributedCache;
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Initializes a new instance of the DistributedCacheProvider
    /// </summary>
    /// <param name="distributedCache">The IDistributedCache instance to use for caching</param>
    /// <exception cref="ArgumentNullException">Thrown when distributedCache is null</exception>
    public DistributedCacheProvider(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache ?? throw new ArgumentNullException(
            nameof(distributedCache),
            "IDistributedCache is not available. Please register a distributed cache provider.");

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
    }

    /// <inheritdoc />
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(key))
            return default(T);

        try
        {
            var data = await _distributedCache.GetStringAsync(key, cancellationToken);
            if (string.IsNullOrEmpty(data))
                return default(T);

            return JsonSerializer.Deserialize<T>(data, _jsonOptions);
        }
        catch (Exception)
        {
            // Return null on cache errors (graceful degradation)
            return default(T);
        }
    }

    /// <inheritdoc />
    public async Task SetAsync<T>(string key, T value, TimeSpan duration, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(key) || value == null)
            return;

        try
        {
            var data = JsonSerializer.Serialize(value, _jsonOptions);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = duration
            };

            await _distributedCache.SetStringAsync(key, data, options, cancellationToken);
        }
        catch (Exception)
        {
            // Silently continue on cache errors
        }
    }

    /// <inheritdoc />
    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(key))
            return;

        try
        {
            await _distributedCache.RemoveAsync(key, cancellationToken);
        }
        catch (Exception)
        {
            // Silently continue on cache errors
        }
    }

    /// <inheritdoc />
    public Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default)
    {
        // IDistributedCache doesn't support pattern-based removal
        // This feature is available in advanced cache providers like Redis
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task ClearAsync(CancellationToken cancellationToken = default)
    {
        // IDistributedCache doesn't support clear all
        // This feature is available in advanced cache providers like Redis
        return Task.CompletedTask;
    }
} 