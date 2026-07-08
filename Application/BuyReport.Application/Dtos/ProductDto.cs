using System.Text.Json.Serialization;
using Model.Dtos;

namespace BuyReport.Application.Dtos;

public class ProductDto : IdDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("cost")]
    public int Cost { get; set; }
}

public class OrderDto : IdDto
{
    [JsonPropertyName("cartId")]
    public Guid CartId { get; set; }
    
    [JsonPropertyName("orderedProductId")]
    public Guid OrderedProductId { get; set; }
    
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}