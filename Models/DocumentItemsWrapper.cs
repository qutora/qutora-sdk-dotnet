using System.Text.Json.Serialization;

namespace Qutora.SDK.Models;

/// <summary>
/// Wrapper for the $values structure in API response
/// </summary>
public class DocumentItemsWrapper
{
    /// <summary>
    /// The actual list of documents
    /// </summary>
    [JsonPropertyName("$values")]
    public List<Document> Values { get; set; } = new();
} 