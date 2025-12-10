using System.Text.Json.Serialization;

namespace Models.Dtos;

public class PlaceDto : BaseDto
{
    [JsonPropertyName("address")]
    public string Address { get; set; }
    
    [JsonPropertyName("workingTime")]
    public string WorkingTime { get; set; }
}