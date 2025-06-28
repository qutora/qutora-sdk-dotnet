using Qutora.SDK.Models;

namespace Qutora.SDK.Interfaces;

/// <summary>
/// Interface for category operations
/// </summary>
public interface ICategoryService
{
    /// <summary>
    /// Gets all categories
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of all categories</returns>
    Task<List<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all categories (alternative endpoint)
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of all categories</returns>
    Task<List<Category>> GetAllCategoriesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a category by its ID
    /// </summary>
    /// <param name="categoryId">Category identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Category details</returns>
    Task<Category> GetCategoryByIdAsync(string categoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new category
    /// </summary>
    /// <param name="name">Category name</param>
    /// <param name="description">Category description</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created category</returns>
    Task<Category> CreateCategoryAsync(string name, string? description = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets root categories (categories without parent)
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of root categories</returns>
    Task<List<Category>> GetRootCategoriesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets subcategories of a specific category
    /// </summary>
    /// <param name="categoryId">Parent category identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of subcategories</returns>
    Task<List<Category>> GetSubcategoriesAsync(string categoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets categories with pagination
    /// </summary>
    /// <param name="page">Page number (1-based)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="searchTerm">Optional search term</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of categories</returns>
    Task<CategoryListResponse> GetCategoriesPagedAsync(int page = 1, int pageSize = 10, string? searchTerm = null, CancellationToken cancellationToken = default);
} 