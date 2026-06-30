using BuyReport.Application.Dtos;
using BuyReport.Application.Interfaces;
using BuyReport.Application.Mappings;
using MediatR;

namespace BuyReport.Application.Features.Commands;

public record CreateBuyReportCommand(Guid CartId, string AuthToken) : IRequest<BuyReportDto>;

public class CreateBuyReportCommandHandler(IBuyReportRepository buyReportRepository, ICartRepository cartRepository) 
    : IRequestHandler<CreateBuyReportCommand, BuyReportDto>
{
    public async Task<BuyReportDto> Handle(CreateBuyReportCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var response = await cartRepository.IsCartExistsAsync(request.CartId, request.AuthToken);
        if (!response) throw new ArgumentException("Invalid cartId");
        var buyReport = await buyReportRepository.CreateBuyReportByCartIdAsync(request.CartId);
        return buyReport.ToDto();
    }
}