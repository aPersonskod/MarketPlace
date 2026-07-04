namespace Orchestrator.Application.Features.Events;

public record CartBuyReportCreatedEvent(Guid CartId, string AuthToken);