using System.Text.Json.Serialization;
using Models;

namespace User.Application.Dto;

public class CreateUserDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    [JsonPropertyName("password")]
    public string Password { get; set; }
    
    [JsonPropertyName("wallet")]
    public int Wallet { get; set; }
    
    [JsonPropertyName("role")]
    public string Role { get; set; }
}