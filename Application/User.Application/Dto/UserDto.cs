using System.Text.Json.Serialization;

namespace User.Application.Dto;

public class UserDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    [JsonPropertyName("wallet")]
    public int Wallet { get; set; }
    
    [JsonPropertyName("role")]
    public string Role { get; set; }
}