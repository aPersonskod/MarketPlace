using System.Text.Json.Serialization;

namespace Models.Dtos;

public class ProductDto : BaseDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("cost")]
    public int Cost { get; set; }
}