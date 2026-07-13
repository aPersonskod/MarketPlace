namespace Orchestrator.Application.Features.Arguments;

public class BuyCartArguments
{
    public Guid CartId { get; set; }
    public string AuthToken { get; set; }
}