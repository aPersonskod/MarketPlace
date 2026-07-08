using System.Text.Json.Serialization;

namespace BuyReport.Application.Dtos;

public class CartForReportDto
{
    [JsonPropertyName("cartId")]
    public Guid CartId { get; set; }
    
    [JsonPropertyName("userId")]
    public Guid UserId { get; set; }
    
    [JsonPropertyName("address")]
    public string Address { get; set; }
    
    [JsonPropertyName("orders")]
    public IEnumerable<OrderForReportDto> Orders { get; set; } = [];
    
    [JsonPropertyName("amountToPay")]
    public int AmountToPay { get; set; }
}