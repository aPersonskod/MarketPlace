using System.Text.Json.Serialization;

namespace Models;

public class Cart
{
    [JsonPropertyName("orders")]
    public IEnumerable<Order> Orders { get; set; }
}