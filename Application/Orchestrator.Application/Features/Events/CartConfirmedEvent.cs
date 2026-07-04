namespace Orchestrator.Application.Features.Events;

public record CartConfirmedEvent(Guid CartId, decimal AmountToPay);
public record CartConfirmingFailedEvent(Guid CartId);