using MediatR;
using Orchestrator.Application.Dtos;
using Orchestrator.Application.Interfaces;

namespace Orchestrator.Application.Features.Commands;

public record CreateBuyReportCommand(CartDto CartDto, string AuthToken) : IRequest<bool>;
public class CreateBuyReportCommandHandler(IBuyReportRepository buyReportRepository) 
    : IRequestHandler<CreateBuyReportCommand, bool>
{
    public async Task<bool> Handle(CreateBuyReportCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var report = await buyReportRepository.CreateBuyReportByCartIdAsync(
            new CreateBuyReportDto(request.CartDto.Id, request.AuthToken));
        return report != null;
    }
}