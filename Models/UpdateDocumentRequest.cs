using System.Text.Json.Serialization;

namespace Qutora.SDK.Models;

/// <summary>
/// Request model for updating a document
/// </summary>
public class UpdateDocumentRequest
{
    /// <summary>
    /// Document identifier
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Document name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Category identifier
    /// </summary>
    [JsonPropertyName("categoryId")]
    public string? CategoryId { get; set; }

    /// <summary>
    /// Bucket identifier
    /// </summary>
    [JsonPropertyName("bucketId")]
    public string? BucketId { get; set; }
} 