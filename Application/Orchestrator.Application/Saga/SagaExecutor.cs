using MassTransit;
using Orchestrator.Application.Features.Arguments;
using Orchestrator.Application.Features.Events;

namespace Orchestrator.Application.Saga;

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