using System.Text.Json.Serialization;

namespace Models.Dtos;

public class Order
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public Guid OrderedProductId { get; set; }
    public int Quantity { get; set; }
}

public class OrderDto : ICloneable
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("orderedProduct")]
    public ProductDto? OrderedProduct { get; set; }
    
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
    public object Clone()
    {
        return new OrderDto()
        {
            Id = Id,
            OrderedProduct = OrderedProduct?.Clone() as ProductDto,
            Quantity = Quantity
        };
    }
}