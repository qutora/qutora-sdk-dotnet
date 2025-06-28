using System.Text.Json.Serialization;

namespace Qutora.SDK.Models;

/// <summary>
/// Represents document metadata
/// </summary>
public class Metadata
{
    /// <summary>
    /// Unique identifier of the metadata
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Document identifier this metadata belongs to
    /// </summary>
    [JsonPropertyName("documentId")]
    public string DocumentId { get; set; } = string.Empty;

    /// <summary>
    /// Metadata values as key-value pairs
    /// </summary>
    [JsonPropertyName("values")]
    public Dictionary<string, object> Values { get; set; } = new();

    /// <summary>
    /// Tags associated with the document
    /// </summary>
    [JsonPropertyName("tags")]
    public List<string> Tags { get; set; } = new();

    /// <summary>
    /// Creation timestamp
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last update timestamp
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Creator user identifier
    /// </summary>
    [JsonPropertyName("createdBy")]
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Schema name
    /// </summary>
    [JsonPropertyName("schemaName")]
    public string? SchemaName { get; set; }

    /// <summary>
    /// Metadata values as key-value pairs (alias for Values for backward compatibility)
    /// </summary>
    [JsonIgnore]
    public Dictionary<string, object> MetadataValues
    {
        get => Values;
        set => Values = value;
    }
} 