namespace Orchestrator.Application.Dtos;

public class ConfirmCartDto
{
    public Guid CartId { get; set; }
    public Guid PlaceId { get; set; }
    public string AuthToken { get; set; }
}