using MassTransit;
using Microsoft.Extensions.Logging;
using Model.SharedExceptions;
using Orchestrator.Application.Dtos;
using Orchestrator.Application.Features.Events;
using Orchestrator.Application.Interfaces;

namespace Orchestrator.Application.Features.Commands;

public class ConfirmCartArguments
{
    public Guid CartId { get; set; }
    public Guid PlaceId { get; set; }
    public string AuthToken { get; set; }
}

public class ConfirmCartLog
{
    public string AuthToken { get; set; }
}

public class ConfirmCartCommandConsumer(ICartRepository cartRepository, ILogger<ConfirmCartCommandConsumer> logger)
    : IConsumer<ConfirmCartArguments>
{
    public async Task Consume(ConsumeContext<ConfirmCartArguments> context)
    {
        try
        {
            var cart = await cartRepository.ConfirmCartAsync(
                context.Message.AuthToken,
                context.Message.PlaceId
            );
            if (cart == null) throw new NotFoundException("Failed to confirm cart");
            var correlationId = cart!.Id;
            await Task.Delay(1000);
            var confirmEvent = new CartConfirmedEvent(
                cart.Id,
                cart.AmountToPay,
                context.Message.AuthToken);
            await context.Publish(confirmEvent, publishContext => publishContext.ConversationId = correlationId);
            /*await context.Publish(
                new CartConfirmedEvent(
                    new Guid(),
                    10,
                    context.Message.ConfirmCartDto.AuthToken)
            );*/
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "Failed to confirm cart {CartId}",
                context.Message.CartId);
            await context.Publish(new CartSubmitFailedEvent(context.Message.CartId,
                context.Message.AuthToken));
        }
    }
}