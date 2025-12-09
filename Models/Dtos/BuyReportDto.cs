using System.Text.Json.Serialization;

namespace Models.Dtos;

public class BuyReport
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public DateTime SaleDate { get; set; }
}

public class BuyReportDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("cart")]
    public Cart Cart { get; set; }
    [JsonPropertyName("sale-date")]
    public DateTime SaleDate { get; set; }
}