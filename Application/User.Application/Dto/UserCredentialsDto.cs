using System.Text.Json.Serialization;

namespace User.Application.Dto;

public class UserCredentialsDto
{
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    [JsonPropertyName("password")]
    public string Password { get; set; }
}