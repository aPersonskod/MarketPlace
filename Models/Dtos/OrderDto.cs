using System.Text.Json.Serialization;

namespace Models.Dtos;

public class OrderDto : BaseDto
{
    [JsonPropertyName("cartId")]
    public Guid CartId { get; set; }
    
    [JsonPropertyName("orderedProductId")]
    public Guid OrderedProductId { get; set; }
    
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}