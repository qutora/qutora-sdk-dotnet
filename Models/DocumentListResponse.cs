using System.Text.Json.Serialization;

namespace Qutora.SDK.Models;

/// <summary>
/// Represents a paginated response containing documents
/// </summary>
public class DocumentListResponse
{
    /// <summary>
    /// List of documents in the current page
    /// </summary>
    [JsonPropertyName("items")]
    public DocumentItemsWrapper Items { get; set; } = new();

    /// <summary>
    /// Total number of documents across all pages
    /// </summary>
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    /// <summary>
    /// Total number of pages
    /// </summary>
    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }

    /// <summary>
    /// Current page number (1-based)
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

    /// <summary>
    /// Gets the actual document list from the wrapper
    /// </summary>
    public List<Document> Documents => Items?.Values ?? new List<Document>();
} 