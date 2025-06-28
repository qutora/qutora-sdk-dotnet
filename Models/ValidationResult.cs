using System.Text.Json.Serialization;

namespace Qutora.SDK.Models;

/// <summary>
/// Represents metadata validation result
/// </summary>
public class ValidationResult
{
    /// <summary>
    /// Indicates if the metadata is valid
    /// </summary>
    [JsonPropertyName("isValid")]
    public bool IsValid { get; set; }

    /// <summary>
    /// Validation error messages
    /// </summary>
    [JsonPropertyName("errors")]
    public List<string> Errors { get; set; } = new();

    /// <summary>
    /// Validation warning messages
    /// </summary>
    [JsonPropertyName("warnings")]
    public List<string> Warnings { get; set; } = new();
} 