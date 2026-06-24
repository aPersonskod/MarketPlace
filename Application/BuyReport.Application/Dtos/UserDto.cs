using System.Text.Json.Serialization;
using Model.Dtos;

namespace BuyReport.Application.Dtos;

public class UserDto : IdDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    [JsonPropertyName("wallet")]
    public int Wallet { get; set; }
    
    [JsonPropertyName("role")]
    public string Role { get; set; }
}