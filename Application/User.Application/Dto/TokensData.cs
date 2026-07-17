using System.Text.Json.Serialization;

namespace User.Application.Dto;

public class TokensData
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }
}