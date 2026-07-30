using MassTransit;
using Microsoft.Extensions.Logging;
using Orchestrator.Application.Dtos;
using Orchestrator.Application.Features.Arguments;
using Orchestrator.Application.Features.SagaLogs;
using Orchestrator.Application.Interfaces;

namespace Orchestrator.Application.Features.Activities;

public class WalletSpendActivity(
    IUserRepository userRepository,
    ILogger<WalletSpendActivity> logger) : IActivity<WalletSpendArguments, WalletSpendLog>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<WalletSpendArguments> context)
    {
        try
        {
            var args = context.Arguments;
            var user = await userRepository.SpendMoney(new UserMoneyDto
            {
                AuthToken = args.AuthToken,
                Money = (int)args.AmountToPay
            });
            var log = new WalletSpendLog()
            {
                CartId = args.CartId,
                AmountToPay = args.AmountToPay,
                AuthToken = args.AuthToken
            };
            var buyCartArguments = new BuyCartArguments()
            {
                CartId = args.CartId,
                AuthToken = args.AuthToken
            };
            return context.CompletedWithVariables(log, buyCartArguments);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while spend money cart");
            throw;
        }
    }

    public async Task<CompensationResult> Compensate(CompensateContext<WalletSpendLog> context)
    {
        var log = context.Log;
        await userRepository.WalletReplenishment(new UserMoneyDto()
        {
            AuthToken = log.AuthToken,
            Money = (int)log.AmountToPay
        });
        return context.Compensated();
    }
}

public class WalletSpendActivityDefinition
    : ActivityDefinition<WalletSpendActivity, WalletSpendArguments, WalletSpendLog>
{
    public WalletSpendActivityDefinition()
    {
        ExecuteEndpointName = "wallet-spend-execute-queue";
        CompensateEndpointName = "wallet-spend-compensate-queue";
    }
}