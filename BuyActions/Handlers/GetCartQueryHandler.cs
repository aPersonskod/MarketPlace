using BuyActions.Queries;
using BuyActions.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using Models.Dtos;
using Models.Extensions;

namespace BuyActions.Handlers;

public class GetCartQueryHandler(IOptions<ShoppingCartSettings> cartOptions)
    : IRequestHandler<GetCartQuery, CartDto?>
{
    public async Task<CartDto?> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return await Task.FromResult<CartDto?>(null);
        
        var query = $"{cartOptions.Value.Address}/GetCartById?cartId={request.CartId}";
        return await query.GetQuery<CartDto>();
    }
}