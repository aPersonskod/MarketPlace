using MediatR;
using Orchestrator.Application.Dtos;
using Orchestrator.Application.Interfaces;
using Orchestrator.Application.Mapping;

namespace Orchestrator.Application.Features.Commands;

public record ConfirmCartCommand(Guid PlaceId, string AuthToken) : IRequest<bool>;
public class ConfirmCartCommandHandler(ICartRepository cartRepository, ISender sender) 
    : IRequestHandler<ConfirmCartCommand, bool>
{
    public async Task<bool> Handle(ConfirmCartCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var confirmedCartDto = await cartRepository.ConfirmCartAsync(request.AuthToken, request.PlaceId);
        
        var walletSpendCommand = new WalletSpendCommand(confirmedCartDto.AmountToPay, request.AuthToken);
        var userDto = await sender.Send(walletSpendCommand, cancellationToken);

        var buyCartCommand = new BuyCartCommand(confirmedCartDto.Id, request.AuthToken);
        var boughtCart = await sender.Send(buyCartCommand, cancellationToken);
        
        var createReportCommand = new CreateBuyReportCommand(boughtCart, request.AuthToken);
        return await sender.Send(createReportCommand, cancellationToken);
    }
}