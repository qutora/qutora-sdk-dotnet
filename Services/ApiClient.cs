using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace Qutora.SDK.Services;

/// <summary>
/// Base HTTP client for making API requests
/// </summary>
internal class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly QutoraClientOptions _options;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly ILogger? _logger;

    public ApiClient(HttpClient httpClient, QutoraClientOptions options, JsonSerializerOptions jsonOptions, ILogger? logger = null)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _jsonOptions = jsonOptions ?? throw new ArgumentNullException(nameof(jsonOptions));
        _logger = logger;
    }

    /// <summary>
    /// Makes a GET request and returns the response as type T
    /// </summary>
    public async Task<T> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default)
    {
        _logger?.LogDebug("GET request to: {Endpoint}", endpoint);
        
        var response = await ExecuteWithRetryAsync(
            () => _httpClient.GetAsync(endpoint, cancellationToken),
            cancellationToken
        );

        return await DeserializeResponseAsync<T>(response);
    }

    /// <summary>
    /// Makes a POST request with JSON body and returns the response as type T
    /// </summary>
    public async Task<T> PostAsync<T>(string endpoint, object? body = null, CancellationToken cancellationToken = default)
    {
        _logger?.LogDebug("POST request to: {Endpoint}", endpoint);

        var content = body != null 
            ? new StringContent(JsonSerializer.Serialize(body, _jsonOptions), Encoding.UTF8, "application/json")
            : null;

        var response = await ExecuteWithRetryAsync(
            () => _httpClient.PostAsync(endpoint, content, cancellationToken),
            cancellationToken
        );

        return await DeserializeResponseAsync<T>(response);
    }

    /// <summary>
    /// Makes a POST request with multipart form data
    /// </summary>
    public async Task<T> PostMultipartAsync<T>(string endpoint, MultipartFormDataContent content, CancellationToken cancellationToken = default)
    {
        _logger?.LogDebug("POST multipart request to: {Endpoint}", endpoint);

        var response = await ExecuteWithRetryAsync(
            () => _httpClient.PostAsync(endpoint, content, cancellationToken),
            cancellationToken
        );

        return await DeserializeResponseAsync<T>(response);
    }

    /// <summary>
    /// Makes a PUT request with JSON body and returns the response as type T
    /// </summary>
    public async Task<T> PutAsync<T>(string endpoint, object body, CancellationToken cancellationToken = default)
    {
        _logger?.LogDebug("PUT request to: {Endpoint}", endpoint);

        var content = new StringContent(JsonSerializer.Serialize(body, _jsonOptions), Encoding.UTF8, "application/json");

        var response = await ExecuteWithRetryAsync(
            () => _httpClient.PutAsync(endpoint, content, cancellationToken),
            cancellationToken
        );

        return await DeserializeResponseAsync<T>(response);
    }

    /// <summary>
    /// Makes a DELETE request
    /// </summary>
    public async Task DeleteAsync(string endpoint, CancellationToken cancellationToken = default)
    {
        _logger?.LogDebug("DELETE request to: {Endpoint}", endpoint);

        var response = await ExecuteWithRetryAsync(
            () => _httpClient.DeleteAsync(endpoint, cancellationToken),
            cancellationToken
        );

        await EnsureSuccessStatusCodeAsync(response);
    }

    /// <summary>
    /// Downloads binary content (for file downloads)
    /// </summary>
    public async Task<byte[]> DownloadBytesAsync(string endpoint, CancellationToken cancellationToken = default)
    {
        _logger?.LogDebug("Download bytes from: {Endpoint}", endpoint);

        var response = await ExecuteWithRetryAsync(
            () => _httpClient.GetAsync(endpoint, cancellationToken),
            cancellationToken
        );

        await EnsureSuccessStatusCodeAsync(response);
        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }

    /// <summary>
    /// Downloads content as stream (for file downloads)
    /// </summary>
    public async Task<Stream> DownloadStreamAsync(string endpoint, CancellationToken cancellationToken = default)
    {
        _logger?.LogDebug("Download stream from: {Endpoint}", endpoint);

        var response = await ExecuteWithRetryAsync(
            () => _httpClient.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead, cancellationToken),
            cancellationToken
        );

        await EnsureSuccessStatusCodeAsync(response);
        return await response.Content.ReadAsStreamAsync(cancellationToken);
    }

    /// <summary>
    /// Executes HTTP request with retry logic
    /// </summary>
    private async Task<HttpResponseMessage> ExecuteWithRetryAsync(
        Func<Task<HttpResponseMessage>> requestFunc,
        CancellationToken cancellationToken)
    {
        var attempt = 0;
        Exception? lastException = null;

        while (attempt <= _options.MaxRetryAttempts)
        {
            try
            {
                var response = await requestFunc();
                
                // If successful or client error (4xx), don't retry
                if (response.IsSuccessStatusCode || ((int)response.StatusCode >= 400 && (int)response.StatusCode < 500))
                {
                    await EnsureSuccessStatusCodeAsync(response);
                    return response;
                }

                // Server error (5xx) - retry if we have attempts left
                if (attempt < _options.MaxRetryAttempts)
                {
                    _logger?.LogWarning("Request failed with {StatusCode}, retrying... (Attempt {Attempt}/{MaxAttempts})", 
                        response.StatusCode, attempt + 1, _options.MaxRetryAttempts);
                    
                    await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)), cancellationToken);
                    attempt++;
                    continue;
                }

                await EnsureSuccessStatusCodeAsync(response);
                return response;
            }
            catch (HttpRequestException ex) when (attempt < _options.MaxRetryAttempts)
            {
                lastException = ex;
                _logger?.LogWarning(ex, "Request failed, retrying... (Attempt {Attempt}/{MaxAttempts})", 
                    attempt + 1, _options.MaxRetryAttempts);
                
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)), cancellationToken);
                attempt++;
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException && attempt < _options.MaxRetryAttempts)
            {
                lastException = ex;
                _logger?.LogWarning(ex, "Request timed out, retrying... (Attempt {Attempt}/{MaxAttempts})", 
                    attempt + 1, _options.MaxRetryAttempts);
                
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)), cancellationToken);
                attempt++;
            }
        }

        throw lastException ?? new HttpRequestException("Request failed after all retry attempts");
    }

    /// <summary>
    /// Deserializes HTTP response content to specified type
    /// </summary>
    private async Task<T> DeserializeResponseAsync<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        
        // _logger?.LogDebug("Response content: {Content}", content); // Removed to reduce log noise

        if (string.IsNullOrWhiteSpace(content))
        {
            return default(T)!;
        }

        try
        {
            return JsonSerializer.Deserialize<T>(content, _jsonOptions)!;
        }
        catch (JsonException ex)
        {
            _logger?.LogError(ex, "Failed to deserialize response content: {Content}", content);
            throw new InvalidOperationException($"Failed to deserialize response: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Ensures the response has a success status code, throws detailed exception if not
    /// </summary>
    private async Task EnsureSuccessStatusCodeAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
            return;

        var content = await response.Content.ReadAsStringAsync();
        var statusCode = (int)response.StatusCode;
        
        _logger?.LogError("HTTP request failed with status {StatusCode}: {Content}", statusCode, content);

        var errorMessage = statusCode switch
        {
            401 => "Authentication failed. Please check your API key and secret.",
            403 => "Access forbidden. You don't have permission to access this resource.",
            404 => "Resource not found.",
            429 => "Rate limit exceeded. Please try again later.",
            >= 500 => "Server error occurred. Please try again later.",
            _ => $"HTTP request failed with status {statusCode}"
        };

        throw new HttpRequestException($"{errorMessage} Response: {content}", null, response.StatusCode);
    }
} 