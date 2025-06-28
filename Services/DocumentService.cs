using Qutora.SDK.Constants;
using Qutora.SDK.Models;
using Qutora.SDK.Interfaces;

namespace Qutora.SDK.Services;

/// <summary>
/// Implementation of document operations
/// </summary>
internal class DocumentService : IDocumentService
{
    private readonly ApiClient _apiClient;

    public DocumentService(ApiClient apiClient)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    /// <inheritdoc />
    public async Task<DocumentListResponse> GetDocumentsAsync(int page = 1, int size = 10, CancellationToken cancellationToken = default)
    {
        return await _apiClient.GetAsync<DocumentListResponse>(ApiEndpoints.Documents.GetDocuments(page, size), cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Document?> GetByIdAsync(string id, bool includeMetadata = false, CancellationToken cancellationToken = default)
    {
        var endpoint = includeMetadata 
            ? $"{ApiEndpoints.Documents.GetById(id)}?includeMetadata=true"
            : ApiEndpoints.Documents.GetById(id);
        return await _apiClient.GetAsync<Document>(endpoint, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Document> CreateDocumentAsync(string fileName, byte[] fileContent, CreateDocumentRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name cannot be null or empty", nameof(fileName));
        if (fileContent == null || fileContent.Length == 0)
            throw new ArgumentException("File content cannot be null or empty", nameof(fileContent));
        if (request == null)
            throw new ArgumentNullException(nameof(request));
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name cannot be null or empty", nameof(request.Name));

        using var content = new MultipartFormDataContent();
        using var fileStreamContent = new ByteArrayContent(fileContent);
        
        fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        content.Add(fileStreamContent, "file", fileName);
        content.Add(new StringContent(request.Name), "name");

        // Add optional parameters
        if (!string.IsNullOrEmpty(request.CategoryId))
            content.Add(new StringContent(request.CategoryId), "categoryId");
        if (!string.IsNullOrEmpty(request.MetadataJson))
            content.Add(new StringContent(request.MetadataJson), "metadataJson");
        if (!string.IsNullOrEmpty(request.MetadataSchemaId))
            content.Add(new StringContent(request.MetadataSchemaId), "metadataSchemaId");
        if (request.CreateShare)
            content.Add(new StringContent(request.CreateShare.ToString().ToLower()), "createShare");
        if (request.ExpiresAfterDays.HasValue)
            content.Add(new StringContent(request.ExpiresAfterDays.Value.ToString()), "expiresAfterDays");
        if (request.MaxViewCount.HasValue)
            content.Add(new StringContent(request.MaxViewCount.Value.ToString()), "maxViewCount");
        if (!string.IsNullOrEmpty(request.Password))
            content.Add(new StringContent(request.Password), "password");
        if (!string.IsNullOrEmpty(request.WatermarkText))
            content.Add(new StringContent(request.WatermarkText), "watermarkText");
        content.Add(new StringContent(request.AllowDownload.ToString().ToLower()), "allowDownload");
        content.Add(new StringContent(request.AllowPrint.ToString().ToLower()), "allowPrint");
        if (!string.IsNullOrEmpty(request.CustomMessage))
            content.Add(new StringContent(request.CustomMessage), "customMessage");
        if (request.NotifyOnAccess)
            content.Add(new StringContent(request.NotifyOnAccess.ToString().ToLower()), "notifyOnAccess");
        if (!string.IsNullOrEmpty(request.NotificationEmails))
            content.Add(new StringContent(request.NotificationEmails), "notificationEmails");
        if (request.IsDirectShare)
            content.Add(new StringContent(request.IsDirectShare.ToString().ToLower()), "isDirectShare");

        // Add query parameters for providerId and bucketId
        var endpoint = ApiEndpoints.Documents.Base;
        var queryParams = new List<string>();
        if (!string.IsNullOrEmpty(request.ProviderId))
            queryParams.Add($"providerId={Uri.EscapeDataString(request.ProviderId)}");
        if (!string.IsNullOrEmpty(request.BucketId))
            queryParams.Add($"bucketId={Uri.EscapeDataString(request.BucketId)}");
        
        if (queryParams.Count > 0)
            endpoint += "?" + string.Join("&", queryParams);

        var response = await _apiClient.PostMultipartAsync<DocumentCreateResponse>(endpoint, content, cancellationToken);
        return response.Document;
    }

    /// <inheritdoc />
    public async Task<Document> CreateDocumentAsync(string fileName, Stream fileStream, CreateDocumentRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name cannot be null or empty", nameof(fileName));
        if (fileStream == null)
            throw new ArgumentNullException(nameof(fileStream));
        if (request == null)
            throw new ArgumentNullException(nameof(request));
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name cannot be null or empty", nameof(request.Name));

        using var content = new MultipartFormDataContent();
        using var streamContent = new StreamContent(fileStream);
        
        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        content.Add(streamContent, "file", fileName);
        content.Add(new StringContent(request.Name), "name");

        // Add optional parameters
        if (!string.IsNullOrEmpty(request.CategoryId))
            content.Add(new StringContent(request.CategoryId), "categoryId");
        if (!string.IsNullOrEmpty(request.MetadataJson))
            content.Add(new StringContent(request.MetadataJson), "metadataJson");
        if (!string.IsNullOrEmpty(request.MetadataSchemaId))
            content.Add(new StringContent(request.MetadataSchemaId), "metadataSchemaId");
        if (request.CreateShare)
            content.Add(new StringContent(request.CreateShare.ToString().ToLower()), "createShare");
        if (request.ExpiresAfterDays.HasValue)
            content.Add(new StringContent(request.ExpiresAfterDays.Value.ToString()), "expiresAfterDays");
        if (request.MaxViewCount.HasValue)
            content.Add(new StringContent(request.MaxViewCount.Value.ToString()), "maxViewCount");
        if (!string.IsNullOrEmpty(request.Password))
            content.Add(new StringContent(request.Password), "password");
        if (!string.IsNullOrEmpty(request.WatermarkText))
            content.Add(new StringContent(request.WatermarkText), "watermarkText");
        content.Add(new StringContent(request.AllowDownload.ToString().ToLower()), "allowDownload");
        content.Add(new StringContent(request.AllowPrint.ToString().ToLower()), "allowPrint");
        if (!string.IsNullOrEmpty(request.CustomMessage))
            content.Add(new StringContent(request.CustomMessage), "customMessage");
        if (request.NotifyOnAccess)
            content.Add(new StringContent(request.NotifyOnAccess.ToString().ToLower()), "notifyOnAccess");
        if (!string.IsNullOrEmpty(request.NotificationEmails))
            content.Add(new StringContent(request.NotificationEmails), "notificationEmails");
        if (request.IsDirectShare)
            content.Add(new StringContent(request.IsDirectShare.ToString().ToLower()), "isDirectShare");

        // Add query parameters for providerId and bucketId
        var endpoint = ApiEndpoints.Documents.Base;
        var queryParams = new List<string>();
        if (!string.IsNullOrEmpty(request.ProviderId))
            queryParams.Add($"providerId={Uri.EscapeDataString(request.ProviderId)}");
        if (!string.IsNullOrEmpty(request.BucketId))
            queryParams.Add($"bucketId={Uri.EscapeDataString(request.BucketId)}");
        
        if (queryParams.Count > 0)
            endpoint += "?" + string.Join("&", queryParams);

        var response = await _apiClient.PostMultipartAsync<DocumentCreateResponse>(endpoint, content, cancellationToken);
        return response.Document;
    }

    /// <summary>
    /// Creates a document with minimal parameters 
    /// </summary>
    public async Task<Document> CreateDocumentAsync(string fileName, byte[] fileContent, string name, string? bucketId = null, CancellationToken cancellationToken = default)
    {
        var request = new CreateDocumentRequest 
        { 
            Name = name,
            BucketId = bucketId
        };
        return await CreateDocumentAsync(fileName, fileContent, request, cancellationToken);
    }

    /// <summary>
    /// Creates a document with minimal parameters 
    /// </summary>
    public async Task<Document> CreateDocumentAsync(string fileName, Stream fileStream, string name, string? bucketId = null, CancellationToken cancellationToken = default)
    {
        var request = new CreateDocumentRequest 
        { 
            Name = name,
            BucketId = bucketId
        };
        return await CreateDocumentAsync(fileName, fileStream, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Document> UpdateDocumentAsync(string documentId, UpdateDocumentRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(documentId))
            throw new ArgumentException("Document ID cannot be null or empty", nameof(documentId));
        if (request == null)
            throw new ArgumentNullException(nameof(request));
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name cannot be null or empty", nameof(request.Name));

        // Set the ID to match the parameter
        request.Id = documentId;

        return await _apiClient.PutAsync<Document>(ApiEndpoints.Documents.GetById(documentId), request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Document> UpdateDocumentAsync(string documentId, string name, string? categoryId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(documentId))
            throw new ArgumentException("Document ID cannot be null or empty", nameof(documentId));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty", nameof(name));

        var request = new UpdateDocumentRequest
        {
            Id = documentId,
            Name = name,
            CategoryId = categoryId
        };

        return await UpdateDocumentAsync(documentId, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<byte[]> DownloadDocumentAsync(string documentId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(documentId))
            throw new ArgumentException("Document ID cannot be null or empty", nameof(documentId));

        return await _apiClient.DownloadBytesAsync(ApiEndpoints.Documents.Download(documentId), cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Stream> DownloadDocumentStreamAsync(string documentId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(documentId))
            throw new ArgumentException("Document ID cannot be null or empty", nameof(documentId));

        return await _apiClient.DownloadStreamAsync(ApiEndpoints.Documents.Download(documentId), cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<Document>> GetDocumentsByCategoryAsync(string categoryId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(categoryId))
            throw new ArgumentException("Category ID cannot be null or empty", nameof(categoryId));

        var response = await _apiClient.GetAsync<DocumentListResponse>(ApiEndpoints.Documents.GetByCategory(categoryId), cancellationToken);
        return response.Items?.Values ?? new List<Document>();
    }

    /// <inheritdoc />
    public async Task<List<DocumentVersion>> GetDocumentVersionsAsync(string documentId, CancellationToken cancellationToken = default)
    {
        var endpoint = ApiEndpoints.Documents.GetVersions(documentId);
        var response = await _apiClient.GetAsync<ValuesResponse<DocumentVersion>>(endpoint, cancellationToken);
        return response?.Values ?? new List<DocumentVersion>();
    }

    public async Task<DocumentListResponse> GetByCategoryAsync(string categoryId, int page = 1, int pageSize = 10)
    {
        var endpoint = $"{ApiEndpoints.Documents.GetByCategory(categoryId)}?page={page}&pageSize={pageSize}";
        return await _apiClient.GetAsync<DocumentListResponse>(endpoint);
    }

    public async Task<DocumentVersion> CreateVersionAsync(string documentId, byte[] fileContent, string fileName, string? changeDescription = null)
    {
        using var content = new MultipartFormDataContent();
        var fileContent_stream = new ByteArrayContent(fileContent);
        fileContent_stream.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        content.Add(fileContent_stream, "file", fileName);
        
        if (!string.IsNullOrEmpty(changeDescription))
        {
            content.Add(new StringContent(changeDescription), "changeDescription");
        }

        return await _apiClient.PostMultipartAsync<DocumentVersion>(ApiEndpoints.Documents.CreateVersion(documentId), content);
    }

    public async Task<DocumentVersion> CreateVersionAsync(string documentId, Stream fileStream, string fileName, string? changeDescription = null)
    {
        using var content = new MultipartFormDataContent();
        var streamContent = new StreamContent(fileStream);
        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        content.Add(streamContent, "file", fileName);
        
        if (!string.IsNullOrEmpty(changeDescription))
        {
            content.Add(new StringContent(changeDescription), "changeDescription");
        }

        return await _apiClient.PostMultipartAsync<DocumentVersion>(ApiEndpoints.Documents.CreateVersion(documentId), content);
    }
} 