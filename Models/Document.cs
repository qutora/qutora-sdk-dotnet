using System.Text.Json.Serialization;

namespace Qutora.SDK.Models;

/// <summary>
/// Represents a document in the Qutora system
/// </summary>
public class Document
{
    /// <summary>
    /// Unique identifier of the document
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Display name of the document
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the document
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Original filename
    /// </summary>
    [JsonPropertyName("fileName")]
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// MIME type of the document
    /// </summary>
    [JsonPropertyName("contentType")]
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// File size in bytes
    /// </summary>
    [JsonPropertyName("fileSize")]
    public long FileSize { get; set; }

    /// <summary>
    /// Human-readable file size
    /// </summary>
    [JsonPropertyName("size")]
    public string Size { get; set; } = string.Empty;

    /// <summary>
    /// Storage path of the document
    /// </summary>
    [JsonPropertyName("storagePath")]
    public string StoragePath { get; set; } = string.Empty;

    /// <summary>
    /// File hash for integrity verification
    /// </summary>
    [JsonPropertyName("hash")]
    public string Hash { get; set; } = string.Empty;

    /// <summary>
    /// Storage provider identifier
    /// </summary>
    [JsonPropertyName("storageProviderId")]
    public string StorageProviderId { get; set; } = string.Empty;

    /// <summary>
    /// Storage provider name
    /// </summary>
    [JsonPropertyName("storageProviderName")]
    public string StorageProviderName { get; set; } = string.Empty;

    /// <summary>
    /// Bucket identifier
    /// </summary>
    [JsonPropertyName("bucketId")]
    public string BucketId { get; set; } = string.Empty;

    /// <summary>
    /// Bucket path
    /// </summary>
    [JsonPropertyName("bucketPath")]
    public string BucketPath { get; set; } = string.Empty;

    /// <summary>
    /// Category identifier
    /// </summary>
    [JsonPropertyName("categoryId")]
    public string? CategoryId { get; set; }

    /// <summary>
    /// Category name
    /// </summary>
    [JsonPropertyName("categoryName")]
    public string? CategoryName { get; set; }

    /// <summary>
    /// Document tags
    /// </summary>
    [JsonPropertyName("tags")]
    public TagsWrapper Tags { get; set; } = new();

    /// <summary>
    /// Last accessed timestamp
    /// </summary>
    [JsonPropertyName("lastAccessedAt")]
    public DateTime? LastAccessedAt { get; set; }

    /// <summary>
    /// Creation timestamp
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last update timestamp
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Creator user identifier
    /// </summary>
    [JsonPropertyName("createdBy")]
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Creator user name
    /// </summary>
    [JsonPropertyName("createdByName")]
    public string CreatedByName { get; set; } = string.Empty;

    /// <summary>
    /// Last modifier user identifier
    /// </summary>
    [JsonPropertyName("modifiedBy")]
    public string? ModifiedBy { get; set; }

    /// <summary>
    /// Soft delete flag
    /// </summary>
    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Deletion timestamp
    /// </summary>
    [JsonPropertyName("deletedAt")]
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// Current version identifier
    /// </summary>
    [JsonPropertyName("currentVersionId")]
    public string CurrentVersionId { get; set; } = string.Empty;

    /// <summary>
    /// Document metadata
    /// </summary>
    [JsonPropertyName("metadata")]
    public object? Metadata { get; set; }

    /// <summary>
    /// Share identifiers
    /// </summary>
    [JsonPropertyName("shareIds")]
    public List<string>? ShareIds { get; set; }

    /// <summary>
    /// File extension
    /// </summary>
    [JsonPropertyName("fileExtension")]
    public string FileExtension { get; set; } = string.Empty;
} 