using System.Text.Json.Serialization;
using Model.Dtos;

namespace Product.Application.Dtos;

public class ProductDto : IdDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("cost")]
    public int Cost { get; set; }
}