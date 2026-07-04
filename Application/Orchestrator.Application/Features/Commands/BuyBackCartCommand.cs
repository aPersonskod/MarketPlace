using MassTransit;
using Orchestrator.Application.Dtos;
using Orchestrator.Application.Interfaces;

namespace Orchestrator.Application.Features.Commands;

public record BuyBackCartCommand(Guid CartId, string AuthToken);
public class BuyBackCartCommandConsumer(ICartRepository cartRepository, IUserRepository userRepository) 
    : IConsumer<BuyBackCartCommand>
{
    public async Task Consume(ConsumeContext<BuyBackCartCommand> context)
    {
        var cart = await cartRepository.BuyBackCartAsync(context.Message.AuthToken, context.Message.CartId);
        await userRepository.WalletReplenishment(new UserMoneyDto()
        {
            Money = cart.AmountToPay,
            AuthToken = context.Message.AuthToken
        });
        await cartRepository.UnConfirmCartAsync(context.Message.AuthToken);
    }
}