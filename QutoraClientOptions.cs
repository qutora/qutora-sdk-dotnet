using Qutora.SDK.Cache;

namespace Qutora.SDK;

/// <summary>
/// Configuration options for the Qutora SDK client
/// </summary>
public class QutoraClientOptions
{
    /// <summary>
    /// Base URL of the Qutora API (e.g., "https://api.qutora.io" or "http://localhost:5099")
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// API Key for authentication
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// API Secret for authentication
    /// </summary>
    public string ApiSecret { get; set; } = string.Empty;

    /// <summary>
    /// Request timeout in seconds (default: 30 seconds)
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Maximum number of retry attempts for failed requests (default: 3)
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    /// User agent string for requests
    /// </summary>
    public string UserAgent { get; set; } = "Qutora.SDK/1.0.0";

    /// <summary>
    /// Additional headers to include in all requests
    /// </summary>
    public Dictionary<string, string> DefaultHeaders { get; set; } = new();

    /// <summary>
    /// Cache configuration
    /// </summary>
    public QutoraCacheConfiguration Cache { get; set; } = new();

    /// <summary>
    /// Validates the configuration options
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when required options are missing or invalid</exception>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(BaseUrl))
            throw new ArgumentException("BaseUrl is required", nameof(BaseUrl));

        if (string.IsNullOrWhiteSpace(ApiKey))
            throw new ArgumentException("ApiKey is required", nameof(ApiKey));

        if (string.IsNullOrWhiteSpace(ApiSecret))
            throw new ArgumentException("ApiSecret is required", nameof(ApiSecret));

        if (TimeoutSeconds <= 0)
            throw new ArgumentException("TimeoutSeconds must be greater than 0", nameof(TimeoutSeconds));

        if (MaxRetryAttempts < 0)
            throw new ArgumentException("MaxRetryAttempts must be 0 or greater", nameof(MaxRetryAttempts));

        if (!Uri.TryCreate(BaseUrl, UriKind.Absolute, out _))
            throw new ArgumentException("BaseUrl must be a valid absolute URI", nameof(BaseUrl));
    }
} 