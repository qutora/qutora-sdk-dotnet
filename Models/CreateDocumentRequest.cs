using System.Text.Json.Serialization;

namespace Qutora.SDK.Models;

/// <summary>
/// Request model for creating a document
/// </summary>
public class CreateDocumentRequest
{
    /// <summary>
    /// Document name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Category identifier (optional)
    /// </summary>
    public string? CategoryId { get; set; }

    /// <summary>
    /// Storage provider identifier (optional)
    /// </summary>
    public string? ProviderId { get; set; }

    /// <summary>
    /// Bucket identifier (optional)
    /// </summary>
    public string? BucketId { get; set; }

    /// <summary>
    /// Metadata as JSON string (optional)
    /// </summary>
    public string? MetadataJson { get; set; }

    /// <summary>
    /// Metadata schema identifier (optional)
    /// </summary>
    public string? MetadataSchemaId { get; set; }

    /// <summary>
    /// Whether to create a share for the document
    /// </summary>
    public bool CreateShare { get; set; } = false;

    /// <summary>
    /// Share expiration in days (optional)
    /// </summary>
    public int? ExpiresAfterDays { get; set; }

    /// <summary>
    /// Maximum view count for share (optional)
    /// </summary>
    public int? MaxViewCount { get; set; }

    /// <summary>
    /// Password for share protection (optional)
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Watermark text (optional)
    /// </summary>
    public string? WatermarkText { get; set; }

    /// <summary>
    /// Allow download for shared document
    /// </summary>
    public bool AllowDownload { get; set; } = true;

    /// <summary>
    /// Allow print for shared document
    /// </summary>
    public bool AllowPrint { get; set; } = true;

    /// <summary>
    /// Custom message for share (optional)
    /// </summary>
    public string? CustomMessage { get; set; }

    /// <summary>
    /// Notify on document access
    /// </summary>
    public bool NotifyOnAccess { get; set; } = false;

    /// <summary>
    /// Notification email addresses (comma-separated)
    /// </summary>
    public string? NotificationEmails { get; set; }

    /// <summary>
    /// Whether this is a direct share
    /// </summary>
    public bool IsDirectShare { get; set; } = false;
} 