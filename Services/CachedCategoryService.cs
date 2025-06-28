using Qutora.SDK.Cache;
using Qutora.SDK.Interfaces;
using Qutora.SDK.Models;

namespace Qutora.SDK.Services;

/// <summary>
/// Cached wrapper for CategoryService that provides automatic caching capabilities
/// </summary>
internal class CachedCategoryService : ICategoryService
{
    private readonly ICategoryService _categoryService;
    private readonly ICacheProvider _cacheProvider;
    private readonly QutoraCacheConfiguration _cacheConfig;

    /// <summary>
    /// Initializes a new instance of the CachedCategoryService
    /// </summary>
    /// <param name="categoryService">The underlying category service</param>
    /// <param name="cacheProvider">The cache provider to use</param>
    /// <param name="cacheConfig">The cache configuration</param>
    /// <exception cref="ArgumentNullException">Thrown when any parameter is null</exception>
    public CachedCategoryService(
        ICategoryService categoryService, 
        ICacheProvider cacheProvider,
        QutoraCacheConfiguration cacheConfig)
    {
        _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        _cacheProvider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider));
        _cacheConfig = cacheConfig ?? throw new ArgumentNullException(nameof(cacheConfig));
    }

    /// <inheritdoc />
    public async Task<List<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await GetCachedAsync(
            "categories:all",
            () => _categoryService.GetCategoriesAsync(cancellationToken),
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<Category>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await GetCachedAsync(
            "categories:all",
            () => _categoryService.GetAllCategoriesAsync(cancellationToken),
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Category> GetCategoryByIdAsync(string categoryId, CancellationToken cancellationToken = default)
    {
        return await GetCachedAsync(
            $"categories:by-id:{categoryId}",
            () => _categoryService.GetCategoryByIdAsync(categoryId, cancellationToken),
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Category> CreateCategoryAsync(string name, string? description = null, CancellationToken cancellationToken = default)
    {
        var result = await _categoryService.CreateCategoryAsync(name, description, cancellationToken);
        
        // Clear cache on Create/Update/Delete operations
        await InvalidateCategoryCacheAsync(cancellationToken);
        
        return result;
    }

    /// <inheritdoc />
    public async Task<List<Category>> GetRootCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await GetCachedAsync(
            "categories:root",
            () => _categoryService.GetRootCategoriesAsync(cancellationToken),
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<Category>> GetSubcategoriesAsync(string categoryId, CancellationToken cancellationToken = default)
    {
        return await GetCachedAsync(
            $"categories:subcategories:{categoryId}",
            () => _categoryService.GetSubcategoriesAsync(categoryId, cancellationToken),
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<CategoryListResponse> GetCategoriesPagedAsync(int page = 1, int pageSize = 10, string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"categories:paged:{page}:{pageSize}:{searchTerm ?? "null"}";
        return await GetCachedAsync(
            cacheKey,
            () => _categoryService.GetCategoriesPagedAsync(page, pageSize, searchTerm, cancellationToken),
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

    private async Task InvalidateCategoryCacheAsync(CancellationToken cancellationToken = default)
    {
        // Clear all category-related caches
        await _cacheProvider.RemoveByPatternAsync($"{_cacheConfig.KeyPrefix}:categories:*", cancellationToken);
    }
} 