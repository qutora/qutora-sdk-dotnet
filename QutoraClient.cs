using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Qutora.SDK.Constants;
using Qutora.SDK.Services;
using System.Text.Json;
using Qutora.SDK.Interfaces;
using Qutora.SDK.Cache;

namespace Qutora.SDK;

/// <summary>
/// Main client for interacting with the Qutora Document Management API
/// </summary>
public class QutoraClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly QutoraClientOptions _options;
    private readonly ILogger<QutoraClient>? _logger;
    private readonly JsonSerializerOptions _jsonOptions;
    private bool _disposed = false;

    /// <summary>
    /// Service for document operations
    /// </summary>
    public IDocumentService Documents { get; }

    /// <summary>
    /// Service for category operations
    /// </summary>
    public ICategoryService Categories { get; }

    /// <summary>
    /// Service for metadata operations
    /// </summary>
    public IMetadataService Metadata { get; }

    /// <summary>
    /// Service for storage operations
    /// </summary>
    public IStorageService Storage { get; }

    /// <summary>
    /// Initializes a new instance of the QutoraClient
    /// </summary>
    /// <param name="options">Configuration options for the client</param>
    /// <param name="httpClient">Optional HttpClient instance. If not provided, a new one will be created.</param>
    /// <param name="logger">Optional logger for debugging and monitoring</param>
    /// <param name="cacheProvider">Optional cache provider for caching responses</param>
    /// <exception cref="ArgumentNullException">Thrown when options is null</exception>
    public QutoraClient(QutoraClientOptions options, HttpClient? httpClient = null, ILogger<QutoraClient>? logger = null, ICacheProvider? cacheProvider = null)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _options.Validate();
        _logger = logger;

        // Configure JSON serialization options
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        // Setup HttpClient
        _httpClient = httpClient ?? new HttpClient();
        ConfigureHttpClient();

        // Initialize services
        var apiClient = new ApiClient(_httpClient, _options, _jsonOptions, _logger);
        
        // Initialize base services
        var baseDocumentService = new DocumentService(apiClient);
        var baseCategoryService = new CategoryService(apiClient);
        var baseMetadataService = new MetadataService(apiClient);
        var baseStorageService = new StorageService(apiClient);

        // Wrap with cache if enabled and cache provider is available
        if (_options.Cache.Enabled && cacheProvider != null)
        {
            Documents = baseDocumentService; // Documents are not cached (large content, dynamic)
            Categories = new CachedCategoryService(baseCategoryService, cacheProvider, _options.Cache);
            Metadata = new CachedMetadataService(baseMetadataService, cacheProvider, _options.Cache);
            Storage = new CachedStorageService(baseStorageService, cacheProvider, _options.Cache);
        }
        else
        {
            Documents = baseDocumentService;
            Categories = baseCategoryService;
            Metadata = baseMetadataService;
            Storage = baseStorageService;
        }

        _logger?.LogInformation("QutoraClient initialized with BaseUrl: {BaseUrl}", _options.BaseUrl);
    }

    /// <summary>
    /// Initializes a new instance of the QutoraClient with simple configuration
    /// </summary>
    /// <param name="baseUrl">Base URL of the Qutora API</param>
    /// <param name="apiKey">API Key for authentication</param>
    /// <param name="apiSecret">API Secret for authentication</param>
    /// <param name="httpClient">Optional HttpClient instance</param>
    /// <param name="logger">Optional logger</param>
    /// <param name="cacheProvider">Optional cache provider</param>
    /// <exception cref="ArgumentException">Thrown when required parameters are null or empty</exception>
    public QutoraClient(string baseUrl, string apiKey, string apiSecret, HttpClient? httpClient = null, ILogger<QutoraClient>? logger = null, ICacheProvider? cacheProvider = null)
        : this(new QutoraClientOptions
        {
            BaseUrl = baseUrl,
            ApiKey = apiKey,
            ApiSecret = apiSecret
        }, httpClient, logger, cacheProvider)
    {
    }

    private void ConfigureHttpClient()
    {
        _httpClient.BaseAddress = new Uri(_options.BaseUrl);
        _httpClient.Timeout = TimeSpan.FromSeconds(_options.TimeoutSeconds);

        // Set authentication headers
        _httpClient.DefaultRequestHeaders.Add("X-QUTORA-Key", _options.ApiKey);
        _httpClient.DefaultRequestHeaders.Add("X-QUTORA-Secret", _options.ApiSecret);

        // Set user agent
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(_options.UserAgent);

        // Add custom headers
        foreach (var header in _options.DefaultHeaders)
        {
            _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
        }

        _logger?.LogDebug("HttpClient configured with BaseUrl: {BaseUrl}, Timeout: {Timeout}s", 
            _options.BaseUrl, _options.TimeoutSeconds);
    }

    /// <summary>
    /// Tests the connection to the Qutora API
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if connection is successful, false otherwise</returns>
    public async Task<bool> TestConnectionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Try to get documents as a simple connectivity test
            var response = await _httpClient.GetAsync(ApiEndpoints.Documents.GetDocuments(1, 1), cancellationToken);
            var isSuccess = response.IsSuccessStatusCode;
            
            _logger?.LogInformation("Connection test result: {IsSuccess} (Status: {StatusCode})", 
                isSuccess, response.StatusCode);
            
            return isSuccess;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Connection test failed");
            return false;
        }
    }

    /// <summary>
    /// Disposes the QutoraClient and releases all resources
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Protected dispose method
    /// </summary>
    /// <param name="disposing">True if disposing managed resources</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _httpClient?.Dispose();
            _disposed = true;
            _logger?.LogDebug("QutoraClient disposed");
        }
    }
} 