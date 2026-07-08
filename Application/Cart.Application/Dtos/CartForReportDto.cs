using System.Text.Json.Serialization;

namespace Cart.Application.Dtos;

public class CartForReportDto
{
    [JsonPropertyName("cartId")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("userId")]
    public Guid UserId { get; set; }
    
    [JsonPropertyName("address")]
    public string Address { get; set; }
    
    [JsonPropertyName("orderId")]
    public Guid OrderId { get; set; }
    
    [JsonPropertyName("productId")]
    public Guid OrderedProductId { get; set; }
    
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
    
    [JsonPropertyName("amountToPay")]
    public int AmountToPay { get; set; }
}

public class CartForReportDetailedDto
{
    [JsonPropertyName("cartId")]
    public Guid CartId { get; set; }
    
    [JsonPropertyName("userId")]
    public Guid UserId { get; set; }
    
    [JsonPropertyName("address")]
    public string Address { get; set; }
    
    [JsonPropertyName("orders")]
    public List<OrderForReportDto> Orders { get; set; } = [];
    
    [JsonPropertyName("amountToPay")]
    public int AmountToPay { get; set; }
}