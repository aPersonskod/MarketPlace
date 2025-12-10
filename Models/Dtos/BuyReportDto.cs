using System.Text.Json.Serialization;

namespace Models.Dtos;

public class BuyReportDto : BaseDto
{
    [JsonPropertyName("cart")]
    public BuyReportCartDto BuyReportCart { get; set; }
    
    [JsonPropertyName("saleDate")]
    public DateTime SaleDate { get; set; }
}

public class BuyReportCartDto : BaseDto
{
    [JsonPropertyName("place")]
    public PlaceDto Place { get; set; }
    
    [JsonPropertyName("user")]
    public UserDto User { get; set; }
    
    [JsonPropertyName("orders")]
    public IEnumerable<BuyReportOrderDto> Orders { get; set; }
    
    [JsonPropertyName("amountToPay")]
    public int AmountToPay { get; set; }
    
    [JsonPropertyName("isConfirmed")]
    public bool IsConfirmed { get; set; }
    
    [JsonPropertyName("isBought")]
    public bool IsBought { get; set; }
}

public class BuyReportOrderDto : BaseDto
{
    [JsonPropertyName("product")]
    public ProductDto Product { get; set; }
    
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}