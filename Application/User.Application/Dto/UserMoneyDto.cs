using System.Text.Json.Serialization;

namespace User.Application.Dto;

public class UserMoneyDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("money")]
    public int Money { get; set; }
}