using System.Text.Json.Serialization;
using Model.Dtos;

namespace BuyReport.Application.Dtos;

public class BuyReportDto : IdDto
{
    [JsonPropertyName("cartId")]
    public Guid CartId { get; set; }
    
    [JsonPropertyName("saleDate")]
    public DateTime SaleDate { get; set; }
}