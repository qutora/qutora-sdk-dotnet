using System.Text.Json.Serialization;

namespace Qutora.SDK.Models;

/// <summary>
/// Represents a document share in the Qutora system
/// </summary>
public class DocumentShare
{
    /// <summary>
    /// Unique identifier of the share
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Share code for accessing the document
    /// </summary>
    [JsonPropertyName("shareCode")]
    public string ShareCode { get; set; } = string.Empty;

    /// <summary>
    /// Document identifier
    /// </summary>
    [JsonPropertyName("documentId")]
    public string DocumentId { get; set; } = string.Empty;

    /// <summary>
    /// Share name/title
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Share description
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Expiration date
    /// </summary>
    [JsonPropertyName("expiresAt")]
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Maximum view count
    /// </summary>
    [JsonPropertyName("maxViewCount")]
    public int? MaxViewCount { get; set; }

    /// <summary>
    /// Current view count
    /// </summary>
    [JsonPropertyName("viewCount")]
    public int ViewCount { get; set; }

    /// <summary>
    /// Password protection
    /// </summary>
    [JsonPropertyName("hasPassword")]
    public bool HasPassword { get; set; }

    /// <summary>
    /// Watermark text
    /// </summary>
    [JsonPropertyName("watermarkText")]
    public string? WatermarkText { get; set; }

    /// <summary>
    /// Allow download
    /// </summary>
    [JsonPropertyName("allowDownload")]
    public bool AllowDownload { get; set; } = true;

    /// <summary>
    /// Allow print
    /// </summary>
    [JsonPropertyName("allowPrint")]
    public bool AllowPrint { get; set; } = true;

    /// <summary>
    /// Custom message
    /// </summary>
    [JsonPropertyName("customMessage")]
    public string? CustomMessage { get; set; }

    /// <summary>
    /// Notify on access
    /// </summary>
    [JsonPropertyName("notifyOnAccess")]
    public bool NotifyOnAccess { get; set; }

    /// <summary>
    /// Notification emails
    /// </summary>
    [JsonPropertyName("notificationEmails")]
    public List<string>? NotificationEmails { get; set; }

    /// <summary>
    /// Is direct share
    /// </summary>
    [JsonPropertyName("isDirectShare")]
    public bool IsDirectShare { get; set; }

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
} 