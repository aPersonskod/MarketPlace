using MassTransit;
using Orchestrator.Application.Features.Events;
using Orchestrator.Application.Interfaces;

namespace Orchestrator.Application.Features.Commands;

public record UnConfirmCartCommand(string AuthToken);
public class UnConfirmCartCommandConsumer(ICartRepository cartRepository) : IConsumer<UnConfirmCartCommand>
{
    public async Task Consume(ConsumeContext<UnConfirmCartCommand> context)
    {
        await cartRepository.UnConfirmCartAsync(context.Message.AuthToken);
    }
}