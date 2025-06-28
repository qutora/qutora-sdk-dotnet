using System.Text.Json.Serialization;

namespace Qutora.SDK.Models;

/// <summary>
/// Wrapper for category list that handles .NET's $values JSON format
/// </summary>
public class CategoryListResponse
{
    /// <summary>
    /// The wrapped category items
    /// </summary>
    [JsonPropertyName("items")]
    public CategoryItemsWrapper Items { get; set; } = new();

    /// <summary>
    /// Total count of categories
    /// </summary>
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    /// <summary>
    /// Total pages
    /// </summary>
    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }

    /// <summary>
    /// Current page number
    /// </summary>
    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; }

    /// <summary>
    /// Page size
    /// </summary>
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    /// <summary>
    /// Gets the actual category list
    /// </summary>
    public List<Category> Categories => Items?.Values ?? new List<Category>();

    /// <summary>
    /// Implicit conversion to List of Category for easier usage
    /// </summary>
    public static implicit operator List<Category>(CategoryListResponse response)
    {
        return response?.Categories ?? new List<Category>();
    }
}

/// <summary>
/// Wrapper for category items array that handles .NET's $values JSON format
/// </summary>
public class CategoryItemsWrapper
{
    /// <summary>
    /// The actual categories array
    /// </summary>
    [JsonPropertyName("$values")]
    public List<Category> Values { get; set; } = new();
} 