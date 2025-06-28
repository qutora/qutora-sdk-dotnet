using System.Text.Json.Serialization;

namespace Qutora.SDK.Models;

/// <summary>
/// Request model for metadata validation
/// </summary>
public class ValidateMetadataRequest
{
    /// <summary>
    /// Schema name for validation
    /// </summary>
    [JsonPropertyName("schemaName")]
    public string SchemaName { get; set; } = string.Empty;

    /// <summary>
    /// Metadata values to validate
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
} 