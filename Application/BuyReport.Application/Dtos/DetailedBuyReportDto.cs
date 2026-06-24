using System.Text.Json.Serialization;

namespace BuyReport.Application.Dtos;

public class DetailedBuyReportDto
{
    [JsonPropertyName("cart")]
    public DetailedCartForReportDto DetailedCartReportDto { get; set; }
    
    [JsonPropertyName("saleDate")]
    public DateTime SaleDate { get; set; }
}