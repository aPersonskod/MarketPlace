namespace Orchestrator.Application.Features.SagaLogs;

public class BuyCartLog
{
    public Guid CartId { get; set; }
    public string AuthToken { get; set; }
}