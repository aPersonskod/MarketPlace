using MediatR;
using Orchestrator.Application.Interfaces;

namespace Orchestrator.Application.Features.Commands;

public record BuyBackCartCommand(Guid CartId, string AuthToken) : IRequest<bool>;
public class BuyBackCartCommandHandler(ICartRepository cartRepository) : IRequestHandler<BuyBackCartCommand, bool>
{
    public async Task<bool> Handle(BuyBackCartCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var cart = await cartRepository.BuyBackCartAsync(request.AuthToken, request.CartId);
        return cart != null;
    }
}