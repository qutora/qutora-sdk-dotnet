using System.Text.Json.Serialization;

namespace Qutora.SDK.Models
{
    /// <summary>
    /// Paginated response for metadata search results with nested values structure
    /// </summary>
    public class MetadataSearchPagedResponse
    {
        [JsonPropertyName("items")]
        public ValuesResponse<MetadataSearchResult> Items { get; set; } = new ValuesResponse<MetadataSearchResult>();

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("pageNumber")]
        public int PageNumber { get; set; }

        [JsonPropertyName("currentPage")]
        public int CurrentPage { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        [JsonPropertyName("hasPreviousPage")]
        public bool HasPreviousPage { get; set; }

        [JsonPropertyName("hasNextPage")]
        public bool HasNextPage { get; set; }
    }
} 