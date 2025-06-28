using System.Text.Json.Serialization;

namespace Qutora.SDK.Models;

/// <summary>
/// Represents a document version
/// </summary>
public class DocumentVersion
{
    /// <summary>
    /// Version identifier
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Document identifier
    /// </summary>
    [JsonPropertyName("documentId")]
    public string DocumentId { get; set; } = string.Empty;

    /// <summary>
    /// Document name
    /// </summary>
    [JsonPropertyName("documentName")]
    public string DocumentName { get; set; } = string.Empty;

    /// <summary>
    /// Version number
    /// </summary>
    [JsonPropertyName("versionNumber")]
    public int VersionNumber { get; set; }

    /// <summary>
    /// File name
    /// </summary>
    [JsonPropertyName("fileName")]
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// File size in bytes
    /// </summary>
    [JsonPropertyName("fileSize")]
    public long FileSize { get; set; }

    /// <summary>
    /// MIME type
    /// </summary>
    [JsonPropertyName("mimeType")]
    public string MimeType { get; set; } = string.Empty;

    /// <summary>
    /// Content type (MIME type)
    /// </summary>
    [JsonPropertyName("contentType")]
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// Size
    /// </summary>
    [JsonPropertyName("size")]
    public string Size { get; set; } = string.Empty;

    /// <summary>
    /// Storage path
    /// </summary>
    [JsonPropertyName("storagePath")]
    public string StoragePath { get; set; } = string.Empty;

    /// <summary>
    /// Creation timestamp
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

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
    /// Change description
    /// </summary>
    [JsonPropertyName("changeDescription")]
    public string ChangeDescription { get; set; } = string.Empty;

    /// <summary>
    /// Whether this is the current version
    /// </summary>
    [JsonPropertyName("isCurrent")]
    public bool IsCurrent { get; set; }

    /// <summary>
    /// Whether this version is active
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }

    /// <summary>
    /// Note
    /// </summary>
    [JsonPropertyName("note")]
    public string Note { get; set; } = string.Empty;
} 