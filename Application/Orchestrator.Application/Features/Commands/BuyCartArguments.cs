using MassTransit;
using Microsoft.Extensions.Logging;
using Orchestrator.Application.Features.Events;
using Orchestrator.Application.Interfaces;

namespace Orchestrator.Application.Features.Commands;

public class BuyCartArguments
{
    public Guid CartId { get; set; }
    public string AuthToken { get; set; }
}

public class BuyCartLog
{
    public Guid CartId { get; set; }
    public string AuthToken { get; set; }
}

public class BuyCartCommandConsumer(ICartRepository cartRepository, ILogger<BuyCartCommandConsumer> logger)
    : IConsumer<BuyCartArguments>
{
    public async Task Consume(ConsumeContext<BuyCartArguments> context)
    {
        try
        {
            //var cart = await cartRepository.BuyCartAsync(context.Message.AuthToken, context.Message.CartId);
            await Task.Delay(1000);
            //await context.Publish(new CartBoughtEvent(cart.Id, cart.AmountToPay, context.Message.AuthToken));
            await context.Publish(new CartBoughtEvent(context.Message.CartId, 10, context.Message.AuthToken));
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