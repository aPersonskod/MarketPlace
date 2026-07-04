using MassTransit;
using Microsoft.Extensions.Logging;
using Orchestrator.Application.Features.Events;
using Orchestrator.Application.Interfaces;

namespace Orchestrator.Application.Features.Commands;

public record BuyCartCommand(Guid CartId, string AuthToken);
public class BuyCartCommandConsumer(ICartRepository cartRepository, ILogger<BuyCartCommandConsumer> logger) 
    : IConsumer<BuyCartCommand>
{
    public async Task Consume(ConsumeContext<BuyCartCommand> context)
    {
        try
        {
            var cart = await cartRepository.BuyCartAsync(context.Message.AuthToken, context.Message.CartId);
            await context.Publish(new CartBoughtEvent(cart.Id, cart.AmountToPay, context.Message.AuthToken));
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "Failed to buy cart {CartId}",
                context.Message.CartId);
            await context.Publish(new CartPaidFailedEvent(context.Message.CartId, context.Message.AuthToken));
        }
    }
}