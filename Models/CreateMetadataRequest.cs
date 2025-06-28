using System.Text.Json.Serialization;

namespace Qutora.SDK.Models;

/// <summary>
/// Request model for creating metadata
/// </summary>
public class CreateMetadataRequest
{
    /// <summary>
    /// Schema name (optional)
    /// </summary>
    [JsonPropertyName("schemaName")]
    public string? SchemaName { get; set; }

    /// <summary>
    /// Metadata values as key-value pairs
    /// </summary>
    [JsonPropertyName("values")]
    public Dictionary<string, object> Values { get; set; } = new();

    /// <summary>
    /// Tags to associate with the document
    /// </summary>
    [JsonPropertyName("tags")]
    public List<string> Tags { get; set; } = new();

    /// <summary>
    /// Metadata values as key-value pairs (alias for Values for backward compatibility)
    /// </summary>
    [JsonIgnore]
    public Dictionary<string, object> Metadata
    {
        get => Values;
        set => Values = value;
    }
} 