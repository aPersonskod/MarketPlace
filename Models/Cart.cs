using System.Text.Json.Serialization;

namespace Models;

public class Cart
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("orders")]
    public List<Order> Orders { get; set; } = new List<Order>();
    [JsonPropertyName("user")]
    public User? User { get; set; }
    [JsonPropertyName("place")]
    public Place? Place { get; set; }
}