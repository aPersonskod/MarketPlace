using MassTransit;
using Orchestrator.Application.Dtos;
using Orchestrator.Application.Features.Events;
using Orchestrator.Application.Interfaces;

namespace Orchestrator.Application.Features.Commands;

public record WalletRefundCommand(Guid CartId, decimal AmountToPay, string AuthToken);
public class WalletRefundCommandConsumer(IUserRepository userRepository, ICartRepository cartRepository) 
    : IConsumer<WalletRefundCommand>
{
    public async Task Consume(ConsumeContext<WalletRefundCommand> context)
    {
        await userRepository.WalletReplenishment(new UserMoneyDto()
        {
            AuthToken = context.Message.AuthToken,
            Money = (int)context.Message.AmountToPay
        });
        await cartRepository.UnConfirmCartAsync(context.Message.AuthToken);
    }
}