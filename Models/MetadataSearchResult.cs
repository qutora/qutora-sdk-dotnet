using System.Text.Json.Serialization;

namespace Qutora.SDK.Models;

/// <summary>
/// Represents metadata search results
/// </summary>
public class MetadataSearchResult
{
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
    /// Associated metadata
    /// </summary>
    [JsonPropertyName("metadata")]
    public Metadata? Metadata { get; set; }

    /// <summary>
    /// Search relevance score
    /// </summary>
    [JsonPropertyName("score")]
    public double Score { get; set; }
} 