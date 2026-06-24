using BuyReport.Application.Dtos;
using BuyReport.Application.Interfaces;
using BuyReport.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Model.Extensions;

namespace BuyReport.Infrastructure.Repositories;

public class CartRepository(IOptions<CartSettings> cartOptions) : ICartRepository
{
    public async Task<IEnumerable<CartForReportDto>?> GetCartsForReportAsync(string? authToken)
    {
        var query = $"{cartOptions.Value.Address}/get-carts-for-report";
        return await query.GetQuery<IEnumerable<CartForReportDto>>(authToken);
    }

    public async Task<bool> IsCartExistsAsync(Guid cartId, string? authToken)
    {
        var query = $"{cartOptions.Value.Address}/get-carts-for-report";
        return await query.GetQuery<bool>(authToken);
    }
}