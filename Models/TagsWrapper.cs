using System.Text.Json.Serialization;

namespace Qutora.SDK.Models;

/// <summary>
/// Wrapper for tags array that handles .NET's $values JSON format
/// </summary>
public class TagsWrapper
{
    /// <summary>
    /// The actual tags array
    /// </summary>
    [JsonPropertyName("$values")]
    public List<string> Values { get; set; } = new();

    /// <summary>
    /// Implicit conversion to List of string for easier usage
    /// </summary>
    public static implicit operator List<string>(TagsWrapper wrapper)
    {
        return wrapper?.Values ?? new List<string>();
    }

    /// <summary>
    /// Implicit conversion from List of string
    /// </summary>
    public static implicit operator TagsWrapper(List<string> tags)
    {
        return new TagsWrapper { Values = tags ?? new List<string>() };
    }
} 