using Qutora.SDK.Models;

namespace Qutora.SDK.Interfaces;

/// <summary>
/// Interface for metadata operations
/// </summary>
public interface IMetadataService
{
    /// <summary>
    /// Gets metadata for a document
    /// </summary>
    /// <param name="documentId">Document identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Document metadata</returns>
    Task<Metadata> GetDocumentMetadataAsync(string documentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates metadata for a document
    /// </summary>
    /// <param name="documentId">Document identifier</param>
    /// <param name="request">Metadata creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created metadata</returns>
    Task<Metadata> CreateDocumentMetadataAsync(string documentId, CreateMetadataRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates metadata for a document
    /// </summary>
    /// <param name="documentId">Document identifier</param>
    /// <param name="request">Metadata update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated metadata</returns>
    Task<Metadata> UpdateDocumentMetadataAsync(string documentId, UpdateMetadataRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches metadata
    /// </summary>
    /// <param name="query">Search query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Search results</returns>
    Task<List<MetadataSearchResult>> SearchMetadataAsync(string query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches metadata with pagination
    /// </summary>
    /// <param name="query">Search query</param>
    /// <param name="page">Page number (1-based)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of metadata search results</returns>
    Task<List<MetadataSearchResult>> SearchMetadataPagedAsync(string query, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all available tags
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of tags</returns>
    Task<List<string>> GetTagsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all available tags with pagination
    /// </summary>
    /// <param name="page">Page number (starting from 1)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paged list of tags</returns>
    Task<PagedResponse<string>> GetTagsPagedAsync(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates metadata values
    /// </summary>
    /// <param name="request">Validation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Validation result</returns>
    Task<ValidationResult> ValidateMetadataAsync(ValidateMetadataRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets metadata schemas with pagination and optional search
    /// </summary>
    /// <param name="page">Page number (1-based)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="query">Optional search query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of metadata schemas</returns>
    Task<List<MetadataSchema>> GetSchemasAsync(int page = 1, int pageSize = 10, string? query = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a metadata schema by ID
    /// </summary>
    /// <param name="schemaId">Schema identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Metadata schema</returns>
    Task<MetadataSchema?> GetSchemaByIdAsync(string schemaId, CancellationToken cancellationToken = default);
} 