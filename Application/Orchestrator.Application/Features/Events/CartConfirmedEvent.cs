namespace Orchestrator.Application.Features.Events;

public record CartConfirmedEvent(Guid CartId, decimal AmountToPay, string AuthToken);
public record CartConfirmingFailedEvent(Guid CartId, string AuthToken);