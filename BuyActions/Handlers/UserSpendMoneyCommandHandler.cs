using BuyActions.Commands;
using BuyActions.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using Models.Dtos;
using Models.Extensions;

namespace BuyActions.Handlers;

public class UserSpendMoneyCommandHandler(IOptions<UserSettings> userOptions)
    : IRequestHandler<UserSpendMoneyCommand, UserDto?>
{
    public async Task<UserDto?> Handle(UserSpendMoneyCommand request, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return await Task.FromResult<UserDto?>(null);

        var query = $"{userOptions.Value.Address}/SpendMoney?userId={request.UserId}&money={request.Money}";
        return await query.PostQuery<UserDto>();
    }
}