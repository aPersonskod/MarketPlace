using System.Text.Json.Serialization;
using Model.Dtos;

namespace Cart.Application.Dtos;

public class CartDto : IdDto
{
    [JsonPropertyName("placeId")]
    public Guid? PlaceId { get; set; }
    
    [JsonPropertyName("userId")]
    public Guid UserId { get; set; }
    
    [JsonPropertyName("amountToPay")]
    public int AmountToPay { get; set; }
    
    [JsonPropertyName("isConfirmed")]
    public bool IsConfirmed { get; set; }
    
    [JsonPropertyName("isBought")]
    public bool IsBought { get; set; }
}