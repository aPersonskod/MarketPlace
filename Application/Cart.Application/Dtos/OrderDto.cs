using System.Text.Json.Serialization;
using Model.Dtos;

namespace Cart.Application.Dtos;

public class OrderDto : IdDto
{
    [JsonPropertyName("cartId")]
    public Guid CartId { get; set; }
    
    [JsonPropertyName("orderedProductId")]
    public Guid OrderedProductId { get; set; }
    
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}

public class CreateOrderDto
{
    [JsonPropertyName("cartId")]
    public Guid CartId { get; set; }
    
    [JsonPropertyName("orderedProductId")]
    public Guid OrderedProductId { get; set; }
    
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}

public class DeleteOrderDto
{
    [JsonPropertyName("cartId")]
    public Guid CartId { get; set; }
    
    [JsonPropertyName("orderedProductId")]
    public Guid OrderedProductId { get; set; }
}