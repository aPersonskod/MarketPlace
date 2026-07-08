using Cart.Application.Dtos;
using Cart.Application.Interfaces.Repositories;
using Cart.Infrastructure.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace Cart.Infrastructure.Repositories;

public class BuyReportRepository(AppDbContext context) : IBuyReportRepository
{
    public async Task<IEnumerable<CartForReportDto>?> GetCartForReportAsync(Guid userId)
    {
        await using var connection = context.Database.GetDbConnection();
        var sql = $"SELECT cart.\"Id\", cart.\"UserId\", place.\"Address\", ord.\"Id\" as OrderId, "+
                  $"ord.\"OrderedProductId\" , ord.\"Quantity\", cart.\"AmountToPay\" " +
                  $"FROM public.\"ShoppingCarts\" as cart " +
                  $"INNER JOIN public.\"Places\" as place ON cart.\"PlaceId\" = place.\"Id\" " +
                  $"RIGHT JOIN public.\"Orders\" as ord ON cart.\"Id\" = ord.\"CartId\" " +
                  $"WHERE cart.\"UserId\" = '{userId}' AND cart.\"IsConfirmed\" = 'true' AND cart.\"IsBought\" = 'true'";
        return await connection.QueryAsync<CartForReportDto>(sql);
    }
}