using System.Text.Json.Serialization;

namespace Models.Dtos;

public class CartDto : BaseDto
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