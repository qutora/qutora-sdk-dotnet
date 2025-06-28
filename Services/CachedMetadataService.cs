using Qutora.SDK.Cache;
using Qutora.SDK.Interfaces;
using Qutora.SDK.Models;

namespace Qutora.SDK.Services;

/// <summary>
/// Cached wrapper for MetadataService that provides automatic caching capabilities
/// </summary>
internal class CachedMetadataService : IMetadataService
{
    private readonly IMetadataService _metadataService;
    private readonly ICacheProvider _cacheProvider;
    private readonly QutoraCacheConfiguration _cacheConfig;

    /// <summary>
    /// Initializes a new instance of the CachedMetadataService
    /// </summary>
    /// <param name="metadataService">The underlying metadata service</param>
    /// <param name="cacheProvider">The cache provider to use</param>
    /// <param name="cacheConfig">The cache configuration</param>
    /// <exception cref="ArgumentNullException">Thrown when any parameter is null</exception>
    public CachedMetadataService(
        IMetadataService metadataService,
        ICacheProvider cacheProvider,
        QutoraCacheConfiguration cacheConfig)
    {
        _metadataService = metadataService ?? throw new ArgumentNullException(nameof(metadataService));
        _cacheProvider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider));
        _cacheConfig = cacheConfig ?? throw new ArgumentNullException(nameof(cacheConfig));
    }

    /// <inheritdoc />
    public async Task<Metadata> GetDocumentMetadataAsync(string documentId, CancellationToken cancellationToken = default)
    {
        return await GetCachedAsync(
            $"metadata:document:{documentId}",
            () => _metadataService.GetDocumentMetadataAsync(documentId, cancellationToken),
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Metadata> CreateDocumentMetadataAsync(string documentId, CreateMetadataRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _metadataService.CreateDocumentMetadataAsync(documentId, request, cancellationToken);
        
        // Clear cache on Create/Update/Delete operations
        await InvalidateMetadataCacheAsync(documentId, cancellationToken);
        
        return result;
    }

    /// <inheritdoc />
    public async Task<Metadata> UpdateDocumentMetadataAsync(string documentId, UpdateMetadataRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _metadataService.UpdateDocumentMetadataAsync(documentId, request, cancellationToken);
        
        // Clear cache on Create/Update/Delete operations
        await InvalidateMetadataCacheAsync(documentId, cancellationToken);
        
        return result;
    }

    /// <inheritdoc />
    public async Task<List<MetadataSearchResult>> SearchMetadataAsync(string query, CancellationToken cancellationToken = default)
    {
        // Search results are not cached as they can change frequently
        return await _metadataService.SearchMetadataAsync(query, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<MetadataSearchResult>> SearchMetadataPagedAsync(string query, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        // Search results are not cached as they can change frequently
        return await _metadataService.SearchMetadataPagedAsync(query, page, pageSize, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<string>> GetTagsAsync(CancellationToken cancellationToken = default)
    {
        return await GetCachedAsync(
            "metadata:tags:all",
            () => _metadataService.GetTagsAsync(cancellationToken),
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PagedResponse<string>> GetTagsPagedAsync(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        return await GetCachedAsync(
            $"metadata:tags:paged:{page}:{pageSize}",
            () => _metadataService.GetTagsPagedAsync(page, pageSize, cancellationToken),
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ValidationResult> ValidateMetadataAsync(ValidateMetadataRequest request, CancellationToken cancellationToken = default)
    {
        // Validation results are not cached as they depend on dynamic input
        return await _metadataService.ValidateMetadataAsync(request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<MetadataSchema>> GetSchemasAsync(int page = 1, int pageSize = 10, string? query = null, CancellationToken cancellationToken = default)
    {
        return await GetCachedAsync(
            $"metadata:schemas:paged:{page}:{pageSize}:{query ?? "null"}",
            () => _metadataService.GetSchemasAsync(page, pageSize, query, cancellationToken),
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<MetadataSchema?> GetSchemaByIdAsync(string schemaId, CancellationToken cancellationToken = default)
    {
        return await GetCachedAsync(
            $"metadata:schema:{schemaId}",
            () => _metadataService.GetSchemaByIdAsync(schemaId, cancellationToken),
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

    private async Task InvalidateMetadataCacheAsync(string? documentId = null, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrEmpty(documentId))
        {
            // Clear specific document metadata cache
            await _cacheProvider.RemoveAsync($"{_cacheConfig.KeyPrefix}:metadata:document:{documentId}", cancellationToken);
        }
        
        // Clear metadata-related caches (tags, schemas might be affected)
        await _cacheProvider.RemoveByPatternAsync($"{_cacheConfig.KeyPrefix}:metadata:tags:*", cancellationToken);
        await _cacheProvider.RemoveByPatternAsync($"{_cacheConfig.KeyPrefix}:metadata:schemas:*", cancellationToken);
    }
} 