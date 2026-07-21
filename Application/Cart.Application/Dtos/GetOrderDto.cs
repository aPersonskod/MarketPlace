using System.Text.Json.Serialization;

namespace Cart.Application.Dtos;

public class GetOrderDto
{
    [JsonPropertyName("cartId")]
    public Guid CartId { get; set; }
    
    [JsonPropertyName("orderedProductId")]
    public Guid OrderedProductId { get; set; }
}