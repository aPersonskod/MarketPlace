using System.Text.Json.Serialization;

namespace Models.Dtos;

public class Cart
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid PlaceId { get; set; }
    public int AmountToPay { get; set; }
    public bool IsConfirmed { get; set; }
    public bool IsBought { get; set; }
}

public class CartDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("orders")]
    public ICollection<Order> Orders { get; set; } = [];
    
    [JsonPropertyName("user")]
    public User User { get; set; }
    
    [JsonPropertyName("place")]
    public Place Place { get; set; }
    
    [JsonPropertyName("amount_to_pay")]
    public int AmountToPay { get; set; }
    
    [JsonPropertyName("is_confirmed")]
    public bool IsConfirmed { get; set; }
    
    [JsonPropertyName("is_bought")]
    public bool IsBought { get; set; }
}