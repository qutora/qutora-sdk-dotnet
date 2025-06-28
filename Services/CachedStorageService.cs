using Qutora.SDK.Cache;
using Qutora.SDK.Interfaces;
using Qutora.SDK.Models;

namespace Qutora.SDK.Services;

/// <summary>
/// Cached wrapper for StorageService that provides automatic caching capabilities
/// </summary>
internal class CachedStorageService : IStorageService
{
    private readonly IStorageService _storageService;
    private readonly ICacheProvider _cacheProvider;
    private readonly QutoraCacheConfiguration _cacheConfig;

    /// <summary>
    /// Initializes a new instance of the CachedStorageService
    /// </summary>
    /// <param name="storageService">The underlying storage service</param>
    /// <param name="cacheProvider">The cache provider to use</param>
    /// <param name="cacheConfig">The cache configuration</param>
    /// <exception cref="ArgumentNullException">Thrown when any parameter is null</exception>
    public CachedStorageService(
        IStorageService storageService,
        ICacheProvider cacheProvider,
        QutoraCacheConfiguration cacheConfig)
    {
        _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        _cacheProvider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider));
        _cacheConfig = cacheConfig ?? throw new ArgumentNullException(nameof(cacheConfig));
    }

    /// <inheritdoc />
    public async Task<PagedResponse<Bucket>> GetAccessibleBucketsAsync(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        return await GetCachedAsync(
            $"storage:buckets:paged:{page}:{pageSize}",
            () => _storageService.GetAccessibleBucketsAsync(page, pageSize, cancellationToken),
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<StorageProvider>> GetAccessibleProvidersAsync(CancellationToken cancellationToken = default)
    {
        return await GetCachedAsync(
            "storage:providers:all",
            () => _storageService.GetAccessibleProvidersAsync(cancellationToken),
            cancellationToken);
    }

    private async Task<T> GetCachedAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default)
    {
        var fullKey = $"{_cacheConfig.KeyPrefix}:{key}";
        
        // Try to get from cache
        var cached = await _cacheProvider.GetAsync<T>(fullKey, cancellationToken);
        if (cached != null)
        {
            return cached;
        }

        // Cache miss - get from API
        var result = await factory();
        
        // Save to cache
        if (result != null)
        {
            await _cacheProvider.SetAsync(fullKey, result, _cacheConfig.DefaultDuration, cancellationToken);
        }

        return result;
    }
} 