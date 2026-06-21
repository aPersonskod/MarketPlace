using System.Text.Json.Serialization;

namespace Product.Application.Dtos;

public class ProductDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("cost")]
    public int Cost { get; set; }
}