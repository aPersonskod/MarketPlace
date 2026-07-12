namespace Orchestrator.Application.Features.Events;

public record CartSubmittedEvent(Guid CartId, Guid PlaceId, string AuthToken);
public record RoutingSlipCompletedEvent(Guid TrackingNumber);
public record RoutingSlipFaultedEvent(Guid TrackingNumber);


public record CartSubmitFailedEvent(Guid CartId, string AuthToken);