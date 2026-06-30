using MediatR;
using Orchestrator.Application.Dtos;
using Orchestrator.Application.Interfaces;
using Orchestrator.Application.Mapping;

namespace Orchestrator.Application.Features.Commands;

public record WalletSpendCommand(int AmountToPay, string AuthToken) : IRequest<UserDto>;
public class WalletSpendCommandHandler(IUserRepository userRepository, 
    ISender sender, UnConfirmCartCommand unConfirmCartCommand) : IRequestHandler<WalletSpendCommand, UserDto>
{
    public async Task<UserDto> Handle(WalletSpendCommand request, CancellationToken cancellationToken)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await userRepository.SpendMoney(new UserMoneyDto()
            {
                AuthToken = request.AuthToken, Money = request.AmountToPay
            });
        }
        catch (Exception e)
        {
            await sender.Send(unConfirmCartCommand, cancellationToken);
            throw;
        }
    }
}