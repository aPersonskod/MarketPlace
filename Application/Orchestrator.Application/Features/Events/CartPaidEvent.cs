namespace Orchestrator.Application.Features.Events;

public record CartPaidEvent(Guid CartId, string AuthToken);
public record CartPaidFailedEvent(Guid CartId, string AuthToken);