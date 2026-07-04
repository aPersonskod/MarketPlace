namespace Orchestrator.Application.Features.Events;

public record CartBoughtEvent(Guid CartId, decimal AmountToPay, string AuthToken);
public record CartBoughtFailedEvent(Guid CartId, decimal AmountToPay, string AuthToken);