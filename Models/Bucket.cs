using System.Text.Json.Serialization;

namespace Qutora.SDK.Models;

/// <summary>
/// Represents a storage bucket
/// </summary>
public class Bucket
{
    /// <summary>
    /// Unique identifier of the bucket
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Bucket name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Bucket path/name
    /// </summary>
    [JsonPropertyName("path")]
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Description of the bucket
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Storage provider identifier
    /// </summary>
    [JsonPropertyName("providerId")]
    public string ProviderId { get; set; } = string.Empty;

    /// <summary>
    /// Storage provider type
    /// </summary>
    [JsonPropertyName("providerType")]
    public string ProviderType { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if this is the default bucket
    /// </summary>
    [JsonPropertyName("isDefault")]
    public bool IsDefault { get; set; }

    /// <summary>
    /// Storage provider identifier
    /// </summary>
    [JsonPropertyName("storageProviderId")]
    public string StorageProviderId { get; set; } = string.Empty;

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