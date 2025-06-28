using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Qutora.SDK.Cache;
using Qutora.SDK.Cache.Providers;
using Qutora.SDK.Interfaces;

namespace Qutora.SDK.Extensions;

/// <summary>
/// Extension methods for IServiceCollection to register Qutora SDK services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Qutora SDK services to the service collection
    /// </summary>
    /// <param name="services">The service collection to add services to</param>
    /// <param name="baseUrl">Base URL of the Qutora API</param>
    /// <param name="apiKey">API Key for authentication</param>
    /// <param name="apiSecret">API Secret for authentication</param>
    /// <param name="enableCache">Whether to enable caching (default: true)</param>
    /// <param name="useDistributedCache">Whether to use distributed cache (default: false)</param>
    /// <param name="cacheMinutes">Cache duration in minutes (default: 15)</param>
    /// <returns>The service collection for method chaining</returns>
    /// <exception cref="ArgumentNullException">Thrown when services is null</exception>
    /// <exception cref="ArgumentException">Thrown when required parameters are null or empty</exception>
    public static IServiceCollection AddQutoraSDK(
        this IServiceCollection services,
        string baseUrl,
        string apiKey,
        string apiSecret,
        bool enableCache = true,
        bool useDistributedCache = false,
        int cacheMinutes = 15)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));
        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new ArgumentException("BaseUrl cannot be null or empty", nameof(baseUrl));
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("ApiKey cannot be null or empty", nameof(apiKey));
        if (string.IsNullOrWhiteSpace(apiSecret))
            throw new ArgumentException("ApiSecret cannot be null or empty", nameof(apiSecret));
        if (cacheMinutes <= 0)
            throw new ArgumentException("Cache duration must be greater than 0", nameof(cacheMinutes));

        // Configure QutoraClientOptions
        var options = new QutoraClientOptions
        {
            BaseUrl = baseUrl,
            ApiKey = apiKey,
            ApiSecret = apiSecret,
            Cache = new QutoraCacheConfiguration
            {
                Enabled = enableCache,
                ProviderType = enableCache ? 
                    (useDistributedCache ? CacheProviderType.Distributed : CacheProviderType.Memory) : 
                    CacheProviderType.None,
                DefaultDuration = TimeSpan.FromMinutes(cacheMinutes)
            }
        };

        return services.AddQutoraSDK(options);
    }

    /// <summary>
    /// Adds Qutora SDK services to the service collection with custom options
    /// </summary>
    /// <param name="services">The service collection to add services to</param>
    /// <param name="options">The Qutora client configuration options</param>
    /// <returns>The service collection for method chaining</returns>
    /// <exception cref="ArgumentNullException">Thrown when services or options is null</exception>
    public static IServiceCollection AddQutoraSDK(
        this IServiceCollection services,
        QutoraClientOptions options)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));
        if (options == null)
            throw new ArgumentNullException(nameof(options));

        options.Validate();

        RegisterCacheProvider(services, options.Cache);

        services.AddSingleton(options);
        services.AddSingleton<QutoraClient>(provider =>
        {
            var clientOptions = provider.GetRequiredService<QutoraClientOptions>();
            var httpClient = provider.GetService<HttpClient>();
            var logger = provider.GetService<ILogger<QutoraClient>>();
            var cacheProvider = provider.GetService<ICacheProvider>();
            
            return new QutoraClient(clientOptions, httpClient, logger, cacheProvider);
        });

        if (!services.Any(x => x.ServiceType == typeof(HttpClient)))
        {
            services.AddHttpClient();
        }

        return services;
    }

    private static void RegisterCacheProvider(IServiceCollection services, QutoraCacheConfiguration cacheConfig)
    {
        if (!cacheConfig.Enabled || cacheConfig.ProviderType == CacheProviderType.None)
        {
            services.AddSingleton<ICacheProvider, NullCacheProvider>();
            return;
        }

        switch (cacheConfig.ProviderType)
        {
            case CacheProviderType.Memory:
                services.AddMemoryCache();
                services.AddSingleton<ICacheProvider, MemoryCacheProvider>();
                break;

            case CacheProviderType.Distributed:
                var distributedCacheDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IDistributedCache));
                if (distributedCacheDescriptor == null)
                {
                    throw new InvalidOperationException(
                        "Distributed cache is requested but IDistributedCache is not registered. " +
                        "Please add a distributed cache provider like Redis or SQL Server cache before calling AddQutoraSDK. " +
                        "Example: services.AddStackExchangeRedisCache(...) or services.AddDistributedSqlServerCache(...)");
                }
                
                services.AddSingleton<ICacheProvider, DistributedCacheProvider>();
                break;

            default:
                services.AddSingleton<ICacheProvider, NullCacheProvider>();
                break;
        }
    }
} 