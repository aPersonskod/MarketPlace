using System.Text.Json.Serialization;

namespace Models.Dtos;

public class Place
{
    public Guid Id { get; set; }
    public string Address { get; set; }
    public string WorkingTime { get; set; }
}

public class PlaceDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("address")]
    public string Address { get; set; }
    
    [JsonPropertyName("working-time")]
    public string WorkingTime { get; set; }
}