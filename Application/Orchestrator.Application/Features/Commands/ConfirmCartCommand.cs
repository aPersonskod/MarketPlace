using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Model.SharedExceptions;
using Orchestrator.Application.Dtos;
using Orchestrator.Application.Features.Events;
using Orchestrator.Application.Interfaces;

namespace Orchestrator.Application.Features.Commands;

public record ConfirmCartCommand(ConfirmCartDto ConfirmCartDto);

public class ConfirmCartCommandConsumer(ICartRepository cartRepository, ILogger<ConfirmCartCommandConsumer> logger)
    : IConsumer<ConfirmCartCommand>
{
    public async Task Consume(ConsumeContext<ConfirmCartCommand> context)
    {
        try
        {
            /*var cart = await cartRepository.ConfirmCartAsync(
                context.Message.ConfirmCartDto.AuthToken,
                context.Message.ConfirmCartDto.PlaceId
            );
            if (cart == null) throw new NotFoundException("Failed to confirm cart");*/
            await Task.Delay(1000);
            /*await context.Publish(
                new CartConfirmedEvent(
                    cart.Id,
                    cart.AmountToPay,
                    context.Message.ConfirmCartDto.AuthToken)
            );*/
            await context.Publish(
                new CartConfirmedEvent(
                    new Guid(),
                    10,
                    context.Message.ConfirmCartDto.AuthToken)
            );
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "Failed to confirm cart {CartId}",
                context.Message.ConfirmCartDto.CartId);
            await context.Publish(new CartSubmitFailedEvent(context.Message.ConfirmCartDto.CartId,
                context.Message.ConfirmCartDto.AuthToken));
        }
    }
}