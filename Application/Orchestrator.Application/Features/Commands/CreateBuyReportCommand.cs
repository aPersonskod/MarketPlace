using MassTransit;
using Microsoft.Extensions.Logging;
using Orchestrator.Application.Dtos;
using Orchestrator.Application.Features.Events;
using Orchestrator.Application.Interfaces;

namespace Orchestrator.Application.Features.Commands;

public record CreateBuyReportCommand(Guid CartId, decimal AmountToPay, string AuthToken);

public class CreateBuyReportCommandConsumer(
    IBuyReportRepository buyReportRepository,
    ILogger<CreateBuyReportCommandConsumer> logger)
    : IConsumer<CreateBuyReportCommand>
{
    public async Task Consume(ConsumeContext<CreateBuyReportCommand> context)
    {
        try
        {
            /*var report = await buyReportRepository.CreateBuyReportAsync(
                new CreateBuyReportDto(context.Message.CartId, context.Message.AuthToken));*/
            await Task.Delay(1000);
            //await context.Publish(new CartBuyReportCreatedEvent(report!.CartId, context.Message.AuthToken));
            await context.Publish(new CartBuyReportCreatedEvent(context.Message.CartId, context.Message.AuthToken));
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "Failed to create buy-report for cart {CartId}",
                context.Message.CartId);
            await context.Publish(new CartBoughtFailedEvent(context.Message.CartId, context.Message.AmountToPay,
                context.Message.AuthToken));
        }
    }
}