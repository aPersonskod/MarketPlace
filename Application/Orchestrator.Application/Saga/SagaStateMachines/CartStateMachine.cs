using MassTransit;
using Orchestrator.Application.Dtos;
using Orchestrator.Application.Features.Commands;
using Orchestrator.Application.Features.Events;
using Orchestrator.Application.Saga.SagaDatas;

namespace Orchestrator.Application.Saga.SagaStateMachines;

public class CartStateMachine : MassTransitStateMachine<CartStateSagaData>
{
    public CartStateMachine()
    {
        Event(() => CartSubmitted, x => x.CorrelateById(c => c.Message.CartId));
        Event(() => CartSubmitFailed, x => x.CorrelateById(c => c.Message.CartId));
        Event(() => CartConfirmed, x => x.CorrelateById(c => c.Message.CartId));
        Event(() => CartConfirmingFailed, x => x.CorrelateById(c => c.Message.CartId));
        Event(() => CartPaid, x => x.CorrelateById(c => c.Message.CartId));
        Event(() => CartPaidFailed, x => x.CorrelateById(c => c.Message.CartId));
        Event(() => CartBought, x => x.CorrelateById(c => c.Message.CartId));
        Event(() => CartBoughtFailed, x => x.CorrelateById(c => c.Message.CartId));
        Event(() => CartBuyReportCreated, x => x.CorrelateById(c => c.Message.CartId));

        InstanceState(x => x.CurrentState);
        Initially(
            When(CartSubmitted)
                .Then(c =>
                {
                    c.Saga.CartId = c.Message.CartId;
                    c.Saga.PlaceId = c.Message.PlaceId;
                    c.Saga.AuthToken = c.Message.AuthToken;
                })
                .TransitionTo(CartConfirmingState)
                .Publish(c => new ConfirmCartCommand(new ConfirmCartDto()
                {
                    CartId = c.Message.CartId,
                    PlaceId = c.Message.PlaceId,
                    AuthToken = c.Message.AuthToken
                }))
        );
        During(CartConfirmingState,
            When(CartConfirmed)
                .Then(c =>
                {
                    c.Saga.IsConfirmed = true;
                    c.Saga.AmountToPay = c.Message.AmountToPay;
                })
                .TransitionTo(UserPayingState)
                .Publish(c => new WalletSpendCommand(c.Message.CartId, c.Saga.AmountToPay, c.Saga.AuthToken)),
            When(CartSubmitFailed)
                .TransitionTo(Failed)
                .Finalize()
        );
        During(UserPayingState,
            When(CartPaid)
                .Then(c => { c.Saga.IsMoneySpent = true; })
                .TransitionTo(CartBuyingState)
                .Publish(c => new BuyCartCommand(c.Message.CartId, c.Message.AuthToken)),
            When(CartConfirmingFailed)
                .Publish(c => new UnConfirmCartCommand(c.Saga.AuthToken))
                .TransitionTo(Failed)
                .Finalize()
        );
        During(CartBuyingState,
            When(CartBought)
                .Then(c => { c.Saga.IsBought = true; })
                .TransitionTo(ReportCreatingState)
                .Publish(c => new CreateBuyReportCommand(c.Message.CartId, c.Message.AmountToPay, c.Message.AuthToken)),
            When(CartPaidFailed)
                .Publish(c => new WalletRefundCommand(c.Message.CartId, c.Saga.AmountToPay, c.Saga.AuthToken))
                .TransitionTo(Failed)
                .Finalize()
        );
        During(ReportCreatingState,
            When(CartBuyReportCreated)
                .Then(c => { c.Saga.IsReportCreated = true; })
                .TransitionTo(CompletedState)
                .Finalize(),
            When(CartBoughtFailed)
                .Publish(c => new BuyBackCartCommand(c.Message.CartId, c.Saga.AuthToken))
                .TransitionTo(Failed)
                .Finalize()
        );
    }

    public State CartConfirmingState { get; private set; }
    public State UserPayingState { get; private set; }
    public State CartBuyingState { get; private set; }
    public State ReportCreatingState { get; private set; }
    public State CompletedState { get; private set; }
    public State Failed { get; private set; }

    public Event<CartSubmittedEvent> CartSubmitted { get; private set; }
    public Event<CartSubmitFailedEvent> CartSubmitFailed { get; private set; }
    public Event<CartConfirmedEvent> CartConfirmed { get; private set; }
    public Event<CartConfirmingFailedEvent> CartConfirmingFailed { get; private set; }
    public Event<CartBoughtEvent> CartBought { get; private set; }
    public Event<CartBoughtFailedEvent> CartBoughtFailed { get; private set; }
    public Event<CartPaidEvent> CartPaid { get; private set; }
    public Event<CartPaidFailedEvent> CartPaidFailed { get; private set; }
    public Event<CartBuyReportCreatedEvent> CartBuyReportCreated { get; private set; }
}