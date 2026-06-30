using MediatR;
using Orchestrator.Application.Dtos;
using Orchestrator.Application.Interfaces;
using Orchestrator.Application.Mapping;

namespace Orchestrator.Application.Features.Commands;

public record BuyCartCommand(Guid CartId, string AuthToken) : IRequest<CartDto>;
public class BuyCartCommandHandler(ICartRepository cartRepository) : IRequestHandler<BuyCartCommand, CartDto>
{
    public async Task<CartDto> Handle(BuyCartCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await cartRepository.BuyCartAsync(request.AuthToken, request.CartId);
    }
}