using MassTransit;
using Orchestrator.Application.Dtos;
using Orchestrator.Application.Features.Events;
using Orchestrator.Application.Interfaces;

namespace Orchestrator.Application.Features.Commands;

public record WalletReplenishmentCommand(Guid CartId, decimal AmountToPay, string AuthToken);
public class WalletReplenishmentCommandConsumer(IUserRepository userRepository) : IConsumer<WalletReplenishmentCommand>
{
    public async Task Consume(ConsumeContext<WalletReplenishmentCommand> context)
    {
        await userRepository.SpendMoney(new UserMoneyDto()
        {
            AuthToken = context.Message.AuthToken,
            Money = (int)context.Message.AmountToPay
        });
        await context.Publish(new CartPaidFailedEvent(context.Message.CartId));
    }
}