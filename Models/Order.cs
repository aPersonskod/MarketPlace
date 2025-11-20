using System.Text.Json.Serialization;

namespace Models;

public class Order : ICloneable
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("orderedProduct")]
    public Product? OrderedProduct { get; set; }
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
    public object Clone()
    {
        return new Order()
        {
            Id = Id,
            OrderedProduct = OrderedProduct?.Clone() as Product,
            Quantity = Quantity
        };
    }
}