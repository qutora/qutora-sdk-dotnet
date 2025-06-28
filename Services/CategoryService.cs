using Qutora.SDK.Constants;
using Qutora.SDK.Models;
using Qutora.SDK.Interfaces;

namespace Qutora.SDK.Services;

/// <summary>
/// Implementation of category operations
/// </summary>
internal class CategoryService : ICategoryService
{
    private readonly ApiClient _apiClient;

    public CategoryService(ApiClient apiClient)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    /// <inheritdoc />
    public async Task<List<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var response = await _apiClient.GetAsync<ValuesResponse<Category>>(ApiEndpoints.Categories.All, cancellationToken);
        return response?.Values ?? new List<Category>();
    }

    /// <inheritdoc />
    public async Task<List<Category>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var response = await _apiClient.GetAsync<ValuesResponse<Category>>(ApiEndpoints.Categories.All, cancellationToken);
        return response?.Values ?? new List<Category>();
    }

    /// <inheritdoc />
    public async Task<Category> GetCategoryByIdAsync(string categoryId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(categoryId))
            throw new ArgumentException("Category ID cannot be null or empty", nameof(categoryId));

        return await _apiClient.GetAsync<Category>(ApiEndpoints.Categories.GetById(categoryId), cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Category> CreateCategoryAsync(string name, string? description = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty", nameof(name));

        var createRequest = new
        {
            name = name,
            description = description
        };

        return await _apiClient.PostAsync<Category>(ApiEndpoints.Categories.Base, createRequest, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<Category>> GetRootCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var response = await _apiClient.GetAsync<ValuesResponse<Category>>(ApiEndpoints.Categories.Root, cancellationToken);
        return response?.Values ?? new List<Category>();
    }

    /// <inheritdoc />
    public async Task<List<Category>> GetSubcategoriesAsync(string categoryId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(categoryId))
            throw new ArgumentException("Category ID cannot be null or empty", nameof(categoryId));

        var response = await _apiClient.GetAsync<ValuesResponse<Category>>(ApiEndpoints.Categories.GetSubcategories(categoryId), cancellationToken);
        return response?.Values ?? new List<Category>();
    }

    /// <inheritdoc />
    public async Task<CategoryListResponse> GetCategoriesPagedAsync(int page = 1, int pageSize = 10, string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        var endpoint = ApiEndpoints.Categories.GetPaged(page, pageSize, searchTerm);
        
        // API PagedDto döner ama JSON'da {"items":{"$values":[...]}} formatında geliyor
        var response = await _apiClient.GetAsync<CategoryPagedResponse>(endpoint, cancellationToken);
        return new CategoryListResponse
        {
            Items = new CategoryItemsWrapper { Values = response?.Items?.Values ?? new List<Category>() },
            TotalCount = response?.TotalCount ?? 0,
            TotalPages = response?.TotalPages ?? 0,
            PageNumber = response?.PageNumber ?? 1,
            PageSize = response?.PageSize ?? pageSize
        };
    }
} 