using MassTransit;
using Orchestrator.Application.Features.Commands;
using Orchestrator.Application.Features.Events;

namespace Orchestrator.Application.Saga.SagaStateMachines;

public class SagaExecutor(IBus bus)
{
    public async Task Execute(CartSubmittedEvent cartSubmitted)
    {
        var builder = new RoutingSlipBuilder(cartSubmitted.CartId);
        builder.AddActivity("ConfirmCartActivity", new Uri("queue:confirm-cart-execute-queue"),
            new ConfirmCartArguments()
            {
                CartId = cartSubmitted.CartId,
                PlaceId = cartSubmitted.PlaceId,
                AuthToken = cartSubmitted.AuthToken
            });
        builder.AddActivity("WalletSpendActivity", new Uri("queue:wallet-spend-execute-queue"));
        builder.AddActivity("BuyCartActivity", new Uri("queue:buy-cart-execute-queue"));
        builder.AddActivity("CreateBuyReportActivity", new Uri("queue:create-buy-report-execute-queue"));
        var routingSlip = builder.Build();
        await bus.Execute(routingSlip);
    }
}

/*public class CartStateMachine : MassTransitStateMachine<CartStateSagaData>
{
    public CartStateMachine()
    {
        InstanceState(x => x.CurrentState);
        
        Event(() => CartSubmitted, x => x.CorrelateById(c => c.Message.CartId));
        Event(() => RoutingSlipCompleted, x => x.CorrelateById(c => c.Message.TrackingNumber));
        Event(() => RoutingSlipFaulted, x => x.CorrelateById(c => c.Message.TrackingNumber));
        
        
        /*Event(() => CartSubmitFailed, x => x.CorrelateById(c => c.Message.CartId));
        Event(() => CartConfirmed, x => x.CorrelateById(c => c.Message.CartId));
        Event(() => CartConfirmingFailed, x => x.CorrelateById(c => c.Message.CartId));
        Event(() => CartPaid, x => x.CorrelateById(c => c.Message.CartId));
        Event(() => CartPaidFailed, x => x.CorrelateById(c => c.Message.CartId));
        Event(() => CartBought, x => x.CorrelateById(c => c.Message.CartId));
        Event(() => CartBoughtFailed, x => x.CorrelateById(c => c.Message.CartId));
        Event(() => CartBuyReportCreated, x => x.CorrelateById(c => c.Message.CartId));#1#

        Initially(
            When(CartSubmitted)
                .Then(c =>
                {
                    c.Saga.CartId = c.Message.CartId;
                    c.Saga.PlaceId = c.Message.PlaceId;
                })
                .ThenAsync(async c =>
                {
                    var builder = new RoutingSlipBuilder(c.Saga.CartId);
                    
                    builder.AddActivity("confirm-cart", new Uri("queue:confirm-cart_execute"), new
                    {
                        c.Saga.CartId,
                        c.Saga.PlaceId,
                        c.Message.AuthToken
                    });
                    var money = 10m;
                    builder.AddActivity("create-buy-report", new Uri("queue:create-buy-report_execute"), new
                    {
                        c.Saga.CartId,
                        money,
                        c.Message.AuthToken
                    });
                    var routingSlip = builder.Build();
                    await c.Execute(routingSlip);
                })
                .TransitionTo(CartConfirmingState)
        );
        During(CartConfirmingState,
            When(RoutingSlipCompleted)
                .TransitionTo(CompletedState),
            When(RoutingSlipFaulted)
                .TransitionTo(Failed)
                .Finalize()
        );
        /*During(UserPayingState,
            When(CartPaid)
                .Then(c => { c.Saga.IsMoneySpent = true; })
                .TransitionTo(CartBuyingState)
                .Publish(c => new BuyCartCommand(c.Message.CartId, c.Message.AuthToken)),
            When(CartConfirmingFailed)
                .Publish(c => new UnConfirmCartCommand(c.Message.AuthToken))
                .TransitionTo(Failed)
                .Finalize()
        );
        During(CartBuyingState,
            When(CartBought)
                .Then(c => { c.Saga.IsBought = true; })
                .TransitionTo(ReportCreatingState)
                .Publish(c => new CreateBuyReportCommand(c.Message.CartId, c.Message.AmountToPay, c.Message.AuthToken)),
            When(CartPaidFailed)
                .Publish(c => new WalletRefundCommand(c.Message.CartId, c.Saga.AmountToPay, c.Message.AuthToken))
                .TransitionTo(Failed)
                .Finalize()
        );
        During(ReportCreatingState,
            When(CartBuyReportCreated)
                .Then(c => { c.Saga.IsReportCreated = true; })
                .TransitionTo(CompletedState)
                .Finalize(),
            When(CartBoughtFailed)
                .Publish(c => new BuyBackCartCommand(c.Message.CartId, c.Message.AuthToken))
                .TransitionTo(Failed)
                .Finalize()
        );
        SetCompletedWhenFinalized();#1#
    }

    public State CartConfirmingState { get; private set; }
    /*public State UserPayingState { get; private set; }
    public State CartBuyingState { get; private set; }
    public State ReportCreatingState { get; private set; }#1#
    public State CompletedState { get; private set; }
    public State Failed { get; private set; }

    public Event<CartSubmittedEvent> CartSubmitted { get; private set; }
    public Event<RoutingSlipCompletedEvent> RoutingSlipCompleted { get; private set; }
    public Event<RoutingSlipFaultedEvent> RoutingSlipFaulted { get; private set; }
    
    
    
    /*public Event<CartSubmitFailedEvent> CartSubmitFailed { get; private set; }
    public Event<CartConfirmedEvent> CartConfirmed { get; private set; }
    public Event<CartConfirmingFailedEvent> CartConfirmingFailed { get; private set; }
    public Event<CartBoughtEvent> CartBought { get; private set; }
    public Event<CartBoughtFailedEvent> CartBoughtFailed { get; private set; }
    public Event<CartPaidEvent> CartPaid { get; private set; }
    public Event<CartPaidFailedEvent> CartPaidFailed { get; private set; }
    public Event<CartBuyReportCreatedEvent> CartBuyReportCreated { get; private set; }#1#
}*/