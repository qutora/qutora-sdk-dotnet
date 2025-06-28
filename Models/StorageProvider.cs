using System.Text.Json.Serialization;

namespace Qutora.SDK.Models;

/// <summary>
/// Represents a storage provider
/// </summary>
public class StorageProvider
{
    /// <summary>
    /// Unique identifier of the storage provider
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Name of the storage provider
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the storage provider
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Type of the storage provider (e.g., "FileSystem", "S3", "Azure")
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Configuration settings for the provider
    /// </summary>
    [JsonPropertyName("configuration")]
    public Dictionary<string, object> Configuration { get; set; } = new();

    /// <summary>
    /// Indicates if the provider is active
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }

    /// <summary>
    /// Indicates if this is the default provider
    /// </summary>
    [JsonPropertyName("isDefault")]
    public bool IsDefault { get; set; }

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
} 