using Qutora.SDK.Models;

namespace Qutora.SDK.Interfaces;

/// <summary>
/// Interface for document operations
/// </summary>
public interface IDocumentService
{
    /// <summary>
    /// Gets a paginated list of documents
    /// </summary>
    /// <param name="page">Page number (1-based)</param>
    /// <param name="size">Number of items per page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated document list</returns>
    Task<DocumentListResponse> GetDocumentsAsync(int page = 1, int size = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a document by its ID
    /// </summary>
    /// <param name="documentId">Document identifier</param>
    /// <param name="includeMetadata">Whether to include metadata</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Document details</returns>
    Task<Document?> GetByIdAsync(string documentId, bool includeMetadata = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new document with full configuration options
    /// </summary>
    /// <param name="fileName">Name of the file</param>
    /// <param name="fileContent">File content as byte array</param>
    /// <param name="request">Document creation request with all options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created document</returns>
    Task<Document> CreateDocumentAsync(string fileName, byte[] fileContent, CreateDocumentRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new document with full configuration options
    /// </summary>
    /// <param name="fileName">Name of the file</param>
    /// <param name="fileStream">File content as stream</param>
    /// <param name="request">Document creation request with all options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created document</returns>
    Task<Document> CreateDocumentAsync(string fileName, Stream fileStream, CreateDocumentRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new document with minimal parameters (backward compatibility)
    /// </summary>
    /// <param name="fileName">Name of the file</param>
    /// <param name="fileContent">File content as byte array</param>
    /// <param name="name">Display name for the document</param>
    /// <param name="bucketId">Storage bucket identifier (optional)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created document</returns>
    Task<Document> CreateDocumentAsync(string fileName, byte[] fileContent, string name, string? bucketId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new document with minimal parameters (backward compatibility)
    /// </summary>
    /// <param name="fileName">Name of the file</param>
    /// <param name="fileStream">File content as stream</param>
    /// <param name="name">Display name for the document</param>
    /// <param name="bucketId">Storage bucket identifier (optional)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created document</returns>
    Task<Document> CreateDocumentAsync(string fileName, Stream fileStream, string name, string? bucketId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing document with full options
    /// </summary>
    /// <param name="documentId">Document identifier</param>
    /// <param name="request">Update request with all options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated document</returns>
    Task<Document> UpdateDocumentAsync(string documentId, UpdateDocumentRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing document's metadata (simple version)
    /// </summary>
    /// <param name="documentId">Document identifier</param>
    /// <param name="name">New display name</param>
    /// <param name="categoryId">Category identifier (optional)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated document</returns>
    Task<Document> UpdateDocumentAsync(string documentId, string name, string? categoryId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads a document's content
    /// </summary>
    /// <param name="documentId">Document identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Document content as byte array</returns>
    Task<byte[]> DownloadDocumentAsync(string documentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads a document's content as a stream
    /// </summary>
    /// <param name="documentId">Document identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Document content as stream</returns>
    Task<Stream> DownloadDocumentStreamAsync(string documentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets documents by category
    /// </summary>
    /// <param name="categoryId">Category identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of documents in the category</returns>
    Task<List<Document>> GetDocumentsByCategoryAsync(string categoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets documents by category with pagination
    /// </summary>
    /// <param name="categoryId">Category identifier</param>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>Paginated document list</returns>
    Task<DocumentListResponse> GetByCategoryAsync(string categoryId, int page = 1, int pageSize = 10);

    /// <summary>
    /// Gets all versions of a document
    /// </summary>
    /// <param name="documentId">Document ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of document versions</returns>
    Task<List<DocumentVersion>> GetDocumentVersionsAsync(string documentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new version of an existing document
    /// </summary>
    /// <param name="documentId">Document identifier</param>
    /// <param name="fileContent">File content as byte array</param>
    /// <param name="fileName">File name</param>
    /// <param name="changeDescription">Description of changes</param>
    /// <returns>Created document version</returns>
    Task<DocumentVersion> CreateVersionAsync(string documentId, byte[] fileContent, string fileName, string? changeDescription = null);

    /// <summary>
    /// Creates a new version of an existing document
    /// </summary>
    /// <param name="documentId">Document identifier</param>
    /// <param name="fileStream">File content as stream</param>
    /// <param name="fileName">File name</param>
    /// <param name="changeDescription">Description of changes</param>
    /// <returns>Created document version</returns>
    Task<DocumentVersion> CreateVersionAsync(string documentId, Stream fileStream, string fileName, string? changeDescription = null);
} 