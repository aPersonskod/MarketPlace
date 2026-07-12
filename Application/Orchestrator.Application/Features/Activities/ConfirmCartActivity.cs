using MassTransit;
using Microsoft.Extensions.Logging;
using Model.SharedExceptions;
using Orchestrator.Application.Features.Commands;
using Orchestrator.Application.Interfaces;

namespace Orchestrator.Application.Features.Activities;

public class ConfirmCartActivity(ICartRepository cartRepository, ILogger<ConfirmCartActivity> logger) 
    : IActivity<ConfirmCartArguments, ConfirmCartLog>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<ConfirmCartArguments> context)
    {
        var args = context.Arguments;
        var cart = await cartRepository.ConfirmCartAsync(
            args.AuthToken,
            args.PlaceId
        );
        if (cart == null) throw new NotFoundException("Cart not found");
        var confirmCartLog = new ConfirmCartLog()
        {
            AuthToken = args.AuthToken
        };
        var walletSpentArgs = new WalletSpendArguments()
        {
            CartId = args.CartId,
            AmountToPay = cart.AmountToPay,
            AuthToken = args.AuthToken
        };
        return context.CompletedWithVariables(confirmCartLog, walletSpentArgs);
    }

    public async Task<CompensationResult> Compensate(CompensateContext<ConfirmCartLog> context)
    {
        var log = context.Log;
        await cartRepository.UnConfirmCartAsync(log.AuthToken);
        return context.Compensated();
    }
}

public class ConfirmCartActivityDefinition 
    : ActivityDefinition<ConfirmCartActivity, ConfirmCartArguments, ConfirmCartLog>
{
    public ConfirmCartActivityDefinition()
    {
        ExecuteEndpointName = "confirm-cart-execute-queue";
        CompensateEndpointName = "confirm-cart-compensate-queue";
    }
}