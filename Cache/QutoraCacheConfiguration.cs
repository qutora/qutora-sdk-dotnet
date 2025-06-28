namespace Qutora.SDK.Cache;

/// <summary>
/// Cache configuration for Qutora SDK
/// </summary>
public class QutoraCacheConfiguration
{
    /// <summary>
    /// Whether caching is enabled (default: true)
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Type of cache provider to use (default: Memory)
    /// </summary>
    public CacheProviderType ProviderType { get; set; } = CacheProviderType.Memory;

    /// <summary>
    /// Default cache duration (default: 15 minutes)
    /// </summary>
    public TimeSpan DefaultDuration { get; set; } = TimeSpan.FromMinutes(15);

    /// <summary>
    /// Cache key prefix (default: "qutora")
    /// </summary>
    public string KeyPrefix { get; set; } = "qutora";

    /// <summary>
    /// Whether to enable cache metrics (default: false)
    /// </summary>
    public bool EnableMetrics { get; set; } = false;
}

/// <summary>
/// Cache provider types
/// </summary>
public enum CacheProviderType
{
    /// <summary>
    /// In-memory cache (IMemoryCache)
    /// </summary>
    Memory,

    /// <summary>
    /// Distributed cache (IDistributedCache)
    /// </summary>
    Distributed,

    /// <summary>
    /// No caching
    /// </summary>
    None
} 