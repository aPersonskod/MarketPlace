using MassTransit;
using Microsoft.Extensions.Logging;
using Orchestrator.Application.Dtos;
using Orchestrator.Application.Features.Events;
using Orchestrator.Application.Interfaces;

namespace Orchestrator.Application.Features.Commands;

public record WalletSpendCommand(Guid CartId, decimal AmountToPay, string AuthToken);
public class WalletSpendCommandConsumer(IUserRepository userRepository, ILogger<WalletSpendCommandConsumer> logger) 
    : IConsumer<WalletSpendCommand>
{
    public async Task Consume(ConsumeContext<WalletSpendCommand> context)
    {
        try
        {
            await userRepository.SpendMoney(new UserMoneyDto()
            {
                AuthToken = context.Message.AuthToken, Money = (int)context.Message.AmountToPay
            });
            await context.Publish(new CartPaidEvent(context.Message.CartId));
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "Failed to process payment for cart {CartId}",
                context.Message.CartId);
            await context.Publish(new CartConfirmingFailedEvent(context.Message.CartId));
        }
    }
}