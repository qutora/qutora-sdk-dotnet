using System.Text.Json.Serialization;

namespace Qutora.SDK.Models;

/// <summary>
/// Response for document creation operation
/// </summary>
public class DocumentCreateResponse
{
    /// <summary>
    /// Created document information
    /// </summary>
    [JsonPropertyName("document")]
    public Document Document { get; set; } = null!;

    /// <summary>
    /// Created share information (if any)
    /// </summary>
    [JsonPropertyName("share")]
    public DocumentShare? Share { get; set; }

    /// <summary>
    /// Was a share created?
    /// </summary>
    public bool HasShare => Share != null;

    /// <summary>
    /// Share URL (if any)
    /// </summary>
    public string? ShareUrl => Share != null ? $"/document/{Share.ShareCode}" : null;
} 