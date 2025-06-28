using System.Text.Json.Serialization;

namespace Qutora.SDK.Models;

/// <summary>
/// Generic paged response model for SDK
/// </summary>
/// <typeparam name="T">The type of items in the response</typeparam>
public class PagedResponse<T>
{
    /// <summary>
    /// List of items
    /// </summary>
    [JsonPropertyName("items")]
    public List<T> Items { get; set; } = new List<T>();

    /// <summary>
    /// Total number of items
    /// </summary>
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    /// <summary>
    /// Total number of pages
    /// </summary>
    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }

    /// <summary>
    /// Current page number
    /// </summary>
    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; }

    /// <summary>
    /// Current page number (alias for PageNumber)
    /// </summary>
    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }

    /// <summary>
    /// Number of items per page
    /// </summary>
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    /// <summary>
    /// Indicates if there is a previous page
    /// </summary>
    [JsonPropertyName("hasPreviousPage")]
    public bool HasPreviousPage { get; set; }

    /// <summary>
    /// Indicates if there is a next page
    /// </summary>
    [JsonPropertyName("hasNextPage")]
    public bool HasNextPage { get; set; }
} 