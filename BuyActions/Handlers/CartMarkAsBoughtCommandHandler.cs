using BuyActions.Commands;
using BuyActions.Settings;
using MediatR;
using Microsoft.Extensions.Options;

namespace BuyActions.Handlers;

public class CartMarkAsBoughtCommandHandler(IOptions<ShoppingCartSettings> cartOptions)
    : IRequestHandler<CartMarkAsBoughtCommand, bool>
{
    public async Task<bool> Handle(CartMarkAsBoughtCommand request, CancellationToken cancellationToken)
    {
        if(cancellationToken.IsCancellationRequested)
            return await Task.FromResult(false);

        var query = $"{cartOptions.Value.Address}/MarkCartAsBought?cartId={request.CartId}";
        using var client = new HttpClient();
        var httpResponse = await client.PostAsync(query, null, cancellationToken);
        httpResponse.EnsureSuccessStatusCode();
        if (!httpResponse.IsSuccessStatusCode)
            throw new ArgumentNullException($"server error code {httpResponse.StatusCode}");
        return await Task.FromResult(true);
    }
}