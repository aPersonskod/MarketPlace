using System.Text.Json.Serialization;
using Model.Dtos;

namespace Cart.Application.Dtos;

public class OrderForReportDto : IdDto
{
    [JsonPropertyName("product")]
    public ProductDto Product { get; set; }
    
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}