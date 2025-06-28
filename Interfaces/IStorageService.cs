using Qutora.SDK.Models;

namespace Qutora.SDK.Interfaces;

/// <summary>
/// Interface for storage operations
/// </summary>
public interface IStorageService
{
    /// <summary>
    /// Gets buckets accessible by the current user with pagination
    /// </summary>
    /// <param name="page">Page number (starting from 1)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paged list of accessible buckets</returns>
    Task<PagedResponse<Bucket>> GetAccessibleBucketsAsync(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets storage providers accessible by the current user
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of accessible storage providers</returns>
    Task<List<StorageProvider>> GetAccessibleProvidersAsync(CancellationToken cancellationToken = default);
} 