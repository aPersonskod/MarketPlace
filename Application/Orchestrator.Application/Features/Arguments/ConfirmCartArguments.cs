namespace Orchestrator.Application.Features.Arguments;

public class ConfirmCartArguments
{
    public Guid CartId { get; set; }
    public Guid PlaceId { get; set; }
    public string AuthToken { get; set; }
}