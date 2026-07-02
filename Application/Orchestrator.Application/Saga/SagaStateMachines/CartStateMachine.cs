using MassTransit;
using Orchestrator.Application.Features.Commands;
using Orchestrator.Application.Features.Events;
using Orchestrator.Application.Saga.SagaDatas;

namespace Orchestrator.Application.Saga.SagaStateMachines;

public class CartStateMachine : MassTransitStateMachine<CartStateSagaData>
{
    public CartStateMachine()
    {
        Event(() => CartSubmitted, x => x.CorrelateById(c => c.Message.CartId));
        Event(() => CartConfirmed, x => x.CorrelateById(c => c.Message.CartId));
        Event(() => CartConfirmingFailed, x => x.CorrelateById(c => c.Message.CartId));
        Event(() => CartPaid, x => x.CorrelateById(c => c.Message.CartId));
        Event(() => CartPaidFailed, x => x.CorrelateById(c => c.Message.CartId));

        InstanceState(x => x.CurrentState);
        Initially(
            When(CartSubmitted)
                .Then(c => { })
                .TransitionTo(ConfirmingState)
                .Publish(c => new ConfirmCartCommand(c.Message.PlaceId, c.Message.AuthToken))
        );
        During(ConfirmingState,
            When(CartConfirmed)
                .Then(c => { c.Saga.IsConfirmed = true; })
                .TransitionTo(PayingState)
                .Publish(c => new WalletSpendCommand(c.Message.CartId, c.Message.AmountToPay, c.Message.AuthToken)),
            When(CartConfirmingFailed)
                .TransitionTo(Failed)
                .Finalize()
        );
    }

    public State ConfirmingState { get; private set; }
    public State PayingState { get; private set; }
    public State ReportCreatingState { get; private set; }
    public State Failed { get; private set; }

    public Event<CartSubmittedEvent> CartSubmitted { get; private set; }
    public Event<CartConfirmedEvent> CartConfirmed { get; private set; }
    public Event<CartConfirmingFailedEvent> CartConfirmingFailed { get; private set; }
    public Event<CartPaidEvent> CartPaid { get; private set; }
    public Event<CartPaidFailedEvent> CartPaidFailed { get; private set; }
}