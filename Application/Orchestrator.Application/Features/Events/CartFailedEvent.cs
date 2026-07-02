namespace Orchestrator.Application.Features.Events;

public record CartFailedEvent(Guid CartId, string Reason);