using BuyActions.Queries;
using BuyActions.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using Models.Dtos;
using Models.Extensions;

namespace BuyActions.Handlers;

public class GetCartOrdersQueryHandler(IOptions<ShoppingCartSettings> cartOptions)
    : IRequestHandler<GetCartOrdersQuery, IEnumerable<OrderDto>?>
{
    public async Task<IEnumerable<OrderDto>?> Handle(GetCartOrdersQuery request, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return await Task.FromResult(Enumerable.Empty<OrderDto>());

        var query = $"{cartOptions.Value.Address}/GetCartOrders?cartId={request.CartId}";
        return await query.GetQuery<IEnumerable<OrderDto>>();
    }
}