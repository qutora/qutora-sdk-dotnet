using System.Text.Json.Serialization;

namespace Qutora.SDK.Models;

/// <summary>
/// Represents a metadata schema
/// </summary>
public class MetadataSchema
{
    /// <summary>
    /// Schema identifier
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Schema name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Schema description
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Schema version
    /// </summary>
    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// Whether the schema is active
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }

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

    /// <summary>
    /// Creator user identifier
    /// </summary>
    [JsonPropertyName("createdBy")]
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Schema fields
    /// </summary>
    [JsonPropertyName("fields")]
    public ValuesResponse<MetadataSchemaField>? Fields { get; set; }

    /// <summary>
    /// File types supported by this schema
    /// </summary>
    [JsonPropertyName("fileTypes")]
    public string[]? FileTypes { get; set; }

    /// <summary>
    /// Category ID this schema belongs to
    /// </summary>
    [JsonPropertyName("categoryId")]
    public string? CategoryId { get; set; }

    /// <summary>
    /// Field count (computed property)
    /// </summary>
    [JsonPropertyName("fieldCount")]
    public int FieldCount { get; set; }
}

/// <summary>
/// Represents a metadata schema field
/// </summary>
public class MetadataSchemaField
{
    /// <summary>
    /// Field name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Display name
    /// </summary>
    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Field description
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Field type (0=Text, 1=Number, etc.)
    /// </summary>
    [JsonPropertyName("type")]
    public int Type { get; set; }

    /// <summary>
    /// Whether the field is required
    /// </summary>
    [JsonPropertyName("isRequired")]
    public bool IsRequired { get; set; }

    /// <summary>
    /// Default value
    /// </summary>
    [JsonPropertyName("defaultValue")]
    public string? DefaultValue { get; set; }

    /// <summary>
    /// Minimum value (for numeric fields)
    /// </summary>
    [JsonPropertyName("minValue")]
    public int? MinValue { get; set; }

    /// <summary>
    /// Maximum value (for numeric fields)
    /// </summary>
    [JsonPropertyName("maxValue")]
    public int? MaxValue { get; set; }

    /// <summary>
    /// Minimum length (for text fields)
    /// </summary>
    [JsonPropertyName("minLength")]
    public int? MinLength { get; set; }

    /// <summary>
    /// Maximum length (for text fields)
    /// </summary>
    [JsonPropertyName("maxLength")]
    public int? MaxLength { get; set; }

    /// <summary>
    /// Validation regex
    /// </summary>
    [JsonPropertyName("validationRegex")]
    public string? ValidationRegex { get; set; }

    /// <summary>
    /// Sort order
    /// </summary>
    [JsonPropertyName("order")]
    public int Order { get; set; }

    /// <summary>
    /// Option items for select/multi-select fields
    /// </summary>
    [JsonPropertyName("optionItems")]
    public ValuesResponse<MetadataFieldOption>? OptionItems { get; set; }
}

/// <summary>
/// Represents a metadata field option
/// </summary>
public class MetadataFieldOption
{
    /// <summary>
    /// Option label
    /// </summary>
    [JsonPropertyName("label")]
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// Option value
    /// </summary>
    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;
} 