using System.Text.Json.Serialization;

namespace Qutora.SDK.Models;

public class StorageProviderListResponse
{
    [JsonPropertyName("$values")]
    public List<StorageProvider> Values { get; set; } = new();
} 