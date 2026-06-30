using MediatR;
using Orchestrator.Application.Interfaces;

namespace Orchestrator.Application.Features.Commands;

public record UnConfirmCartCommand(string AuthToken) : IRequest<bool>;
public class UnConfirmCartCommandHandler(ICartRepository cartRepository) : IRequestHandler<UnConfirmCartCommand, bool>
{
    public async Task<bool> Handle(UnConfirmCartCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var cart = await cartRepository.UnConfirmCartAsync(request.AuthToken);
        return cart != null;
    }
}