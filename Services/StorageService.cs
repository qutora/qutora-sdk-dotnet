using Qutora.SDK.Constants;
using Qutora.SDK.Models;
using Qutora.SDK.Interfaces;

namespace Qutora.SDK.Services;

/// <summary>
/// Implementation of storage operations
/// </summary>
internal class StorageService : IStorageService
{
    private readonly ApiClient _apiClient;

    public StorageService(ApiClient apiClient)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    /// <inheritdoc />
    public async Task<PagedResponse<Bucket>> GetAccessibleBucketsAsync(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var endpoint = $"{ApiEndpoints.Storage.AccessibleBuckets}?page={page}&pageSize={pageSize}";
        var response = await _apiClient.GetAsync<BucketListResponse>(endpoint, cancellationToken);
        
        return new PagedResponse<Bucket>
        {
            Items = response.Items.Values,
            TotalCount = response.TotalCount,
            TotalPages = response.TotalPages,
            PageNumber = response.PageNumber,
            CurrentPage = response.CurrentPage,
            PageSize = response.PageSize,
            HasPreviousPage = response.HasPreviousPage,
            HasNextPage = response.HasNextPage
        };
    }

    /// <inheritdoc />
    public async Task<List<StorageProvider>> GetAccessibleProvidersAsync(CancellationToken cancellationToken = default)
    {
        var response = await _apiClient.GetAsync<StorageProviderListResponse>(ApiEndpoints.Storage.UserAccessibleProviders, cancellationToken);
        return response.Values;
    }
} 