using System.Text.Json.Serialization;

namespace BuyReport.Application.Dtos;

public class DetailedCartForReportDto
{
    [JsonPropertyName("cartId")]
    public Guid CartId { get; set; }
    
    [JsonPropertyName("user")]
    public UserDto User { get; set; }
    
    [JsonPropertyName("address")]
    public string Address { get; set; }
    
    [JsonPropertyName("products")]
    public IEnumerable<ProductDto> Products { get; set; }
    
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
    
    [JsonPropertyName("amountToPay")]
    public int AmountToPay { get; set; }
}