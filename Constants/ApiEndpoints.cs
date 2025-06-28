namespace Qutora.SDK.Constants;

/// <summary>
/// API endpoint constants for the Qutora SDK
/// </summary>
internal static class ApiEndpoints
{
    /// <summary>
    /// Base API path
    /// </summary>
    public const string ApiBase = "/api";

    /// <summary>
    /// Health check endpoint
    /// </summary>
    public const string Health = $"{ApiBase}/health";

    /// <summary>
    /// Document-related endpoints
    /// </summary>
    public static class Documents
    {
        /// <summary>
        /// Base documents endpoint
        /// </summary>
        public const string Base = $"{ApiBase}/documents";

        /// <summary>
        /// Get documents with pagination
        /// </summary>
        public static string GetDocuments(int page = 1, int size = 10) => $"{Base}?page={page}&size={size}";

        /// <summary>
        /// Get document by ID
        /// </summary>
        public static string GetById(string documentId) => $"{Base}/{documentId}";

        /// <summary>
        /// Download document
        /// </summary>
        public static string Download(string documentId) => $"{Base}/{documentId}/download";

        /// <summary>
        /// Get documents by category
        /// </summary>
        public static string GetByCategory(string categoryId) => $"{Base}/category/{categoryId}";

        /// <summary>
        /// Get document versions
        /// </summary>
        public static string GetVersions(string documentId) => $"{Base}/{documentId}/versions";

        /// <summary>
        /// Create new document version
        /// </summary>
        public static string CreateVersion(string documentId) => $"{Base}/{documentId}/versions";
    }

    /// <summary>
    /// Category-related endpoints
    /// </summary>
    public static class Categories
    {
        /// <summary>
        /// Base categories endpoint
        /// </summary>
        public const string Base = $"{ApiBase}/categories";

        /// <summary>
        /// Get category by ID
        /// </summary>
        public static string GetById(string categoryId) => $"{Base}/{categoryId}";

        /// <summary>
        /// Get all categories
        /// </summary>
        public const string All = $"{Base}/all";

        /// <summary>
        /// Get root categories
        /// </summary>
        public const string Root = $"{Base}/root";

        /// <summary>
        /// Get paged categories
        /// </summary>
        public static string GetPaged(int page, int pageSize, string? searchTerm = null)
        {
            var query = $"page={page}&pageSize={pageSize}";
            if (!string.IsNullOrEmpty(searchTerm))
                query += $"&searchTerm={Uri.EscapeDataString(searchTerm)}";
            return $"{Base}?{query}";
        }

        /// <summary>
        /// Get subcategories
        /// </summary>
        public static string GetSubcategories(string id) => $"{Base}/{id}/subcategories";
    }

    /// <summary>
    /// Metadata-related endpoints
    /// </summary>
    public static class Metadata
    {
        /// <summary>
        /// Base metadata endpoint
        /// </summary>
        public const string Base = $"{ApiBase}/metadata";

        /// <summary>
        /// Get document metadata
        /// </summary>
        public static string GetDocumentMetadata(string documentId) => $"{Base}/document/{documentId}";

        /// <summary>
        /// Create document metadata
        /// </summary>
        public static string CreateForDocument(string documentId) => $"{Base}/document/{documentId}";

        /// <summary>
        /// Update document metadata
        /// </summary>
        public static string UpdateForDocument(string documentId) => $"{Base}/document/{documentId}";

        /// <summary>
        /// Search metadata
        /// </summary>
        public static string Search(string searchCriteriaJson) => $"{Base}/search?searchCriteriaJson={Uri.EscapeDataString(searchCriteriaJson)}";

        /// <summary>
        /// Search metadata with pagination
        /// </summary>
        public static string SearchPaged(string searchCriteriaJson, int page, int pageSize) => $"{Base}/search?searchCriteriaJson={Uri.EscapeDataString(searchCriteriaJson)}&page={page}&pageSize={pageSize}";

        /// <summary>
        /// Get metadata by tags
        /// </summary>
        public static string GetByTags(string tags) => $"{Base}/tags?tags={Uri.EscapeDataString(tags)}";

        /// <summary>
        /// Get metadata by tags with pagination
        /// </summary>
        public static string GetByTagsPaged(string tags, int page, int pageSize) => $"{Base}/tags?tags={Uri.EscapeDataString(tags)}&page={page}&pageSize={pageSize}";

        /// <summary>
        /// Get all tags
        /// </summary>
        public const string Tags = $"{Base}/tags";

        /// <summary>
        /// Validate metadata
        /// </summary>
        public const string Validate = $"{Base}/validate";

        /// <summary>
        /// Validate metadata with schema
        /// </summary>
        public static string ValidateWithSchema(string schemaName) => $"{Base}/validate?schemaName={Uri.EscapeDataString(schemaName)}";

        /// <summary>
        /// Metadata schemas
        /// </summary>
        public const string Schemas = $"{Base}/schemas";

        /// <summary>
        /// Get metadata schema by ID
        /// </summary>
        public static string GetSchemaById(string schemaId) => $"{Schemas}/{schemaId}";

        /// <summary>
        /// Get metadata schemas with pagination
        /// </summary>
        public static string GetSchemasPaged(int page, int pageSize, string? query = null)
        {
            var queryString = $"page={page}&pageSize={pageSize}";
            if (!string.IsNullOrEmpty(query))
                queryString += $"&query={Uri.EscapeDataString(query)}";
            return $"{Schemas}?{queryString}";
        }
    }

    /// <summary>
    /// Storage-related endpoints
    /// </summary>
    public static class Storage
    {
        /// <summary>
        /// Base storage endpoint
        /// </summary>
        public const string Base = $"{ApiBase}/storage";

        /// <summary>
        /// Get user accessible buckets
        /// </summary>
        public const string AccessibleBuckets = $"{Base}/buckets/my-accessible-buckets";

        /// <summary>
        /// Get user accessible storage providers
        /// </summary>
        public const string UserAccessibleProviders = $"{Base}/providers/user-accessible";
    }
} 