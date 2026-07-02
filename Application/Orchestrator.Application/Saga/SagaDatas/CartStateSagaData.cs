using MassTransit;

namespace Orchestrator.Application.Saga.SagaDatas;

public class CartStateSagaData : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    // business data
    public bool IsConfirmed { get; set; }
    public bool IsBought { get; set; }
    public bool IsReportCreated { get; set; }
}