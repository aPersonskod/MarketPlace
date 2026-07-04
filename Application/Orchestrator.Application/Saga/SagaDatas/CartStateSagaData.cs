using MassTransit;

namespace Orchestrator.Application.Saga.SagaDatas;

public class CartStateSagaData : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    // business data
    public Guid CartId { get; set; }
    public Guid PlaceId { get; set; }
    public decimal AmountToPay { get; set; }
    public bool IsConfirmed { get; set; }
    public bool IsMoneySpent { get; set; }
    public bool IsBought { get; set; }
    public bool IsReportCreated { get; set; }
}