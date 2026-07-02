using MassTransit;

namespace Orchestrator.Application.Dtos;

public class UserMoneyDto
{
    public string AuthToken { get; set; }
    public int Money { get; set; }
}
// command todo remove this all
public record ConfirmOrder(Guid CartId);
public record PayOrder(Guid UserId, int Money);
public record CreateReport(Guid CartId);
// event
public record OrderSent(Guid CartId);
public record OrderConfirmed
{
    public Guid UserId { get; init; }
    public Guid CartId { get; init; }
    public int Money { get; init; }
}

public record PaymentProcessed(Guid UserId);
public record ReportCreated(Guid CartId);
public record OrderFailed(Guid CartId, string Reason);

// implement command 
public class OrderConfirmedHandler : IConsumer<ConfirmOrder>
{
    public async Task Consume(ConsumeContext<ConfirmOrder> context)
    {
        // update database with dbContext
        var money = 100; // get amount to pay from db 
        // publish that order is confirmed (use event)
        await context.Publish(new OrderConfirmed()
        {
            CartId = context.Message.CartId,
            Money = money
        });
    }
}

public class OrderStateSagaData : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public Guid UserId { get; set; }
    public Guid CartId { get; set; }
    public bool IsConfirmed { get; set; }
    public bool IsBought { get; set; }
    public bool IsReportCreated { get; set; }
    public string? FailureReason { get; set; }
}

public class OrderStateMachine : MassTransitStateMachine<OrderStateSagaData>
{
    public OrderStateMachine()
    {
        Event(() => OrderSent, x => x.CorrelateById(c => c.Message.CartId));
        Event(() => OrderConfirmed, x => x.CorrelateById(c => c.Message.CartId));
        Event(() => PaymentProcessed, x => x.CorrelateById(c => c.Message.UserId));
        Event(() => ReportCreated, x => x.CorrelateById(c => c.Message.CartId));
        // and another...
        
        InstanceState(x => x.CurrentState);
        Initially(When(OrderSent)
            .Then(c =>
            {
                c.Saga.CartId = c.Message.CartId;
            })
            .TransitionTo(Confirming)
            .Publish(c => new ConfirmOrder(c.Message.CartId))
        );
        During(Confirming, When(OrderConfirmed)
            .Then(c =>
            {
                c.Saga.UserId = c.Message.UserId;
                c.Saga.IsConfirmed = true;
            })
            .TransitionTo(Paying)
            .Publish(c => new PayOrder(c.Message.UserId, c.Message.Money))
        );
        During(Paying, When(PaymentProcessed)
            .Then(c =>
            {
                c.Saga.IsBought = true;
                c.Saga.IsReportCreated = true;
            })
            .TransitionTo(CreatingReport)
            .Publish(c => new CreateReport(c.Saga.CartId))
            .Finalize()
        );
    }
    
    public State Confirming { get; private set; }
    public State Paying { get; private set; }
    public State CreatingReport { get; private set; }
    
    public Event<OrderSent> OrderSent { get; private set; }
    public Event<OrderConfirmed> OrderConfirmed { get; private set; }
    public Event<PaymentProcessed> PaymentProcessed { get; private set; }
    public Event<ReportCreated> ReportCreated { get; private set; }
    public Event<OrderFailed> OrderFailed { get; private set; }
}