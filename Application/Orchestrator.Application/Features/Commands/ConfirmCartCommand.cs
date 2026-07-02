using MassTransit;
using Orchestrator.Application.Features.Events;
using Orchestrator.Application.Interfaces;

namespace Orchestrator.Application.Features.Commands;

public record ConfirmCartCommand(Guid PlaceId, string AuthToken);
public class ConfirmCartCommandConsumer(ICartRepository cartRepository) : IConsumer<ConfirmCartCommand>
{
    public async Task Consume(ConsumeContext<ConfirmCartCommand> context)
    {
        var cart = await cartRepository.ConfirmCartAsync(context.Message.AuthToken, context.Message.PlaceId);
        await context.Publish(new CartConfirmedEvent(cart.Id, cart.AmountToPay, context.Message.AuthToken));
    }
}