using System.Text.Json.Serialization;

namespace Orchestrator.Application.Dtos;

public class CartSubmittedDto
{
    [JsonPropertyName("cartId")]
    public Guid CartId { get; set; }
    [JsonPropertyName("placeId")]
    public Guid PlaceId { get; set; }
}