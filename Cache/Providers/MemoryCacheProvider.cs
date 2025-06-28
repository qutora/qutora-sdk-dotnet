using Microsoft.Extensions.Caching.Memory;
using Qutora.SDK.Interfaces;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Qutora.SDK.Cache.Providers;

/// <summary>
/// Memory cache provider implementation using IMemoryCache
/// </summary>
public class MemoryCacheProvider : ICacheProvider
{
    private readonly IMemoryCache _memoryCache;
    private readonly ConcurrentDictionary<string, bool> _keys;

    /// <summary>
    /// Initializes a new instance of the MemoryCacheProvider
    /// </summary>
    /// <param name="memoryCache">The IMemoryCache instance to use for caching</param>
    /// <exception cref="ArgumentNullException">Thrown when memoryCache is null</exception>
    public MemoryCacheProvider(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        _keys = new ConcurrentDictionary<string, bool>();
    }

    /// <inheritdoc />
    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(key))
            return Task.FromResult(default(T));

        try
        {
            var value = _memoryCache.Get<T>(key);
            return Task.FromResult(value);
        }
        catch (Exception)
        {
            return Task.FromResult(default(T));
        }
    }

    /// <inheritdoc />
    public Task SetAsync<T>(string key, T value, TimeSpan duration, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(key) || value == null)
            return Task.CompletedTask;

        try
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = duration,
                Priority = CacheItemPriority.Normal
            };

            options.RegisterPostEvictionCallback((evictedKey, evictedValue, reason, state) =>
            {
                _keys.TryRemove(evictedKey.ToString()!, out _);
            });

            _memoryCache.Set(key, value, options);
            _keys.TryAdd(key, true);
        }
        catch (Exception)
        {
            // Silently continue on cache errors
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(key))
            return Task.CompletedTask;

        try
        {
            _memoryCache.Remove(key);
            _keys.TryRemove(key, out _);
        }
        catch (Exception)
        {
            // Silently continue on cache errors
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(pattern))
            return Task.CompletedTask;

        try
        {
            // Escape regex special characters except * which we convert to .*
            var escapedPattern = Regex.Escape(pattern).Replace("\\*", ".*");
            var regex = new Regex($"^{escapedPattern}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            var keysToRemove = _keys.Keys.Where(key => regex.IsMatch(key)).ToList();

            foreach (var key in keysToRemove)
            {
                _memoryCache.Remove(key);
                _keys.TryRemove(key, out _);
            }
        }
        catch (Exception)
        {
            // Silently continue on cache errors
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task ClearAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var keysToRemove = _keys.Keys.ToList();
            foreach (var key in keysToRemove)
            {
                _memoryCache.Remove(key);
            }
            _keys.Clear();
        }
        catch (Exception)
        {
            // Silently continue on cache errors
        }

        return Task.CompletedTask;
    }
} 