using BuyActions.Queries;
using BuyActions.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using Models.Dtos;
using Models.Extensions;

namespace BuyActions.Handlers;

public class GetUserQueryHandler(IOptions<UserSettings> userOptions) : IRequestHandler<GetUserQuery, UserDto?>
{
    public async Task<UserDto?> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        if(cancellationToken.IsCancellationRequested)
            return await Task.FromResult<UserDto?>(null);
        
        var query = $"{userOptions.Value.Address}";
        return await query.GetQuery<UserDto>(request.AccessToken);
    }
}