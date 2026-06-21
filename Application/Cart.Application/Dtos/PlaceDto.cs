using System.Text.Json.Serialization;
using Model.Dtos;

namespace Cart.Application.Dtos;

public class PlaceDto : IdDto
{
    [JsonPropertyName("address")]
    public string Address { get; set; }
    
    [JsonPropertyName("workingTime")]
    public string WorkingTime { get; set; }
}