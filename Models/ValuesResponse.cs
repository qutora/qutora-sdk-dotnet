using System.Text.Json.Serialization;

namespace Qutora.SDK.Models
{
    /// <summary>
    /// Response wrapper for endpoints that return data in $values format
    /// </summary>
    /// <typeparam name="T">The type of items in the values array</typeparam>
    public class ValuesResponse<T>
    {
        [JsonPropertyName("$values")]
        public List<T> Values { get; set; } = new List<T>();
    }
} 