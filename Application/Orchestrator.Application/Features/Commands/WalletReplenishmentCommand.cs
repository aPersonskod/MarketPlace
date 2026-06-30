using MediatR;
using Orchestrator.Application.Dtos;
using Orchestrator.Application.Interfaces;

namespace Orchestrator.Application.Features.Commands;

public record WalletReplenishmentCommand(int AmountToPay, string AuthToken) : IRequest<bool>;
public class WalletReplenishmentCommandHandler(IUserRepository userRepository) 
    : IRequestHandler<WalletReplenishmentCommand, bool>
{
    public async Task<bool> Handle(WalletReplenishmentCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var user = await userRepository.SpendMoney(new UserMoneyDto()
        {
            AuthToken = request.AuthToken, Money = request.AmountToPay
        });
        return user != null;
    }
}