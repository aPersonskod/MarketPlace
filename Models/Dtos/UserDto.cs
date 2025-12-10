using System.Text.Json.Serialization;

namespace Models.Dtos;

public class UserDto : BaseDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    [JsonPropertyName("wallet")]
    public int Wallet { get; set; }
}