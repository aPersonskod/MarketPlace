using MassTransit;
using Model.SharedExceptions;
using Orchestrator.Application.Features.Arguments;
using Orchestrator.Application.Features.SagaLogs;
using Orchestrator.Application.Interfaces;

namespace Orchestrator.Application.Features.Activities;

public class BuyCartActivity(ICartRepository cartRepository) : IActivity<BuyCartArguments, BuyCartLog>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<BuyCartArguments> context)
    {
        var args = context.Arguments;
        var cart = await cartRepository.BuyCartAsync(args.AuthToken, args.CartId);
        if (cart == null) throw new NotFoundException("Cart not found");
        var log = new BuyCartLog()
        {
            CartId = args.CartId,
            AuthToken = args.AuthToken
        };
        var buyReportArguments = new CreateBuyReportArguments()
        {
            CartId = args.CartId,
            AmountToPay = cart.AmountToPay,
            AuthToken = args.AuthToken
        };
        return context.CompletedWithVariables(log, buyReportArguments);
    }

    public async Task<CompensationResult> Compensate(CompensateContext<BuyCartLog> context)
    {
        var log = context.Log;
        await cartRepository.BuyBackCartAsync(log.AuthToken, log.CartId);
        return context.Compensated();
    }
}

public class BuyCartActivityDefinition : ActivityDefinition<BuyCartActivity, BuyCartArguments, BuyCartLog>
{
    public BuyCartActivityDefinition()
    {
        ExecuteEndpointName = "buy-cart-execute-queue";
        CompensateEndpointName = "buy-cart-compensate-queue";
    }
}