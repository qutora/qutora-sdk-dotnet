using Qutora.SDK.Constants;
using Qutora.SDK.Models;
using Qutora.SDK.Interfaces;

namespace Qutora.SDK.Services;

/// <summary>
/// Implementation of metadata operations
/// </summary>
internal class MetadataService : IMetadataService
{
    private readonly ApiClient _apiClient;

    public MetadataService(ApiClient apiClient)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    /// <inheritdoc />
    public async Task<Metadata> GetDocumentMetadataAsync(string documentId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(documentId))
            throw new ArgumentException("Document ID cannot be null or empty", nameof(documentId));

        return await _apiClient.GetAsync<Metadata>(ApiEndpoints.Metadata.GetDocumentMetadata(documentId), cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Metadata> CreateDocumentMetadataAsync(string documentId, CreateMetadataRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(documentId))
            throw new ArgumentException("Document ID cannot be null or empty", nameof(documentId));
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        return await _apiClient.PostAsync<Metadata>(ApiEndpoints.Metadata.GetDocumentMetadata(documentId), request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Metadata> UpdateDocumentMetadataAsync(string documentId, UpdateMetadataRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(documentId))
            throw new ArgumentException("Document ID cannot be null or empty", nameof(documentId));
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        return await _apiClient.PutAsync<Metadata>(ApiEndpoints.Metadata.GetDocumentMetadata(documentId), request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<MetadataSearchResult>> SearchMetadataAsync(string query, CancellationToken cancellationToken = default)
    {
        var searchCriteria = new { query = query };
        var searchJson = System.Text.Json.JsonSerializer.Serialize(searchCriteria);
        var endpoint = ApiEndpoints.Metadata.Search(searchJson);
        
        var response = await _apiClient.GetAsync<MetadataSearchPagedResponse>(endpoint, cancellationToken);
        return response?.Items?.Values ?? new List<MetadataSearchResult>();
    }

    /// <inheritdoc />
    public async Task<List<MetadataSearchResult>> SearchMetadataPagedAsync(string query, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var searchCriteria = new { query = query, page = page, pageSize = pageSize };
        var searchJson = System.Text.Json.JsonSerializer.Serialize(searchCriteria);
        var endpoint = ApiEndpoints.Metadata.SearchPaged(searchJson, page, pageSize);
        
        var response = await _apiClient.GetAsync<MetadataSearchPagedResponse>(endpoint, cancellationToken);
        return response?.Items?.Values ?? new List<MetadataSearchResult>();
    }

    /// <inheritdoc />
    public async Task<List<string>> GetTagsAsync(CancellationToken cancellationToken = default)
    {
        // Tüm tag'ları almak için direkt tags endpoint'ini kullan
        var endpoint = ApiEndpoints.Metadata.Tags;
        
        try
        {
            var response = await _apiClient.GetAsync<List<string>>(endpoint, cancellationToken);
            return response ?? new List<string>();
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("tags field is required"))
        {
            // API tags parametresi gerektiriyorsa, boş bir liste döndür ve uyarı ver
            return new List<string>();
        }
    }

    /// <inheritdoc />
    public async Task<PagedResponse<string>> GetTagsPagedAsync(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        // Paginated tags için direkt endpoint kullan
        var endpoint = $"{ApiEndpoints.Metadata.Tags}?page={page}&pageSize={pageSize}";
        
        try
        {
            var response = await _apiClient.GetAsync<PagedResponse<string>>(endpoint, cancellationToken);
            return response ?? new PagedResponse<string>();
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("tags field is required"))
        {
            // API tags parametresi gerektiriyorsa, boş bir response döndür
            return new PagedResponse<string>();
        }
    }

    /// <inheritdoc />
    public async Task<ValidationResult> ValidateMetadataAsync(ValidateMetadataRequest request, CancellationToken cancellationToken = default)
    {
        var endpoint = ApiEndpoints.Metadata.ValidateWithSchema(request.SchemaName);
        var response = await _apiClient.PostAsync<ValidationResult>(endpoint, request.Metadata, cancellationToken);
        return response ?? new ValidationResult { IsValid = false };
    }

    /// <inheritdoc />
    public async Task<List<MetadataSchema>> GetSchemasAsync(int page = 1, int pageSize = 10, string? query = null, CancellationToken cancellationToken = default)
    {
        var endpoint = ApiEndpoints.Metadata.GetSchemasPaged(page, pageSize, query);
        
        var response = await _apiClient.GetAsync<MetadataSchemaPagedResponse>(endpoint, cancellationToken);
        return response?.Items?.Values ?? new List<MetadataSchema>();
    }

    /// <inheritdoc />
    public async Task<MetadataSchema?> GetSchemaByIdAsync(string schemaId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(schemaId))
            throw new ArgumentException("Schema ID cannot be null or empty", nameof(schemaId));

        return await _apiClient.GetAsync<MetadataSchema>(ApiEndpoints.Metadata.GetSchemaById(schemaId), cancellationToken);
    }
} 