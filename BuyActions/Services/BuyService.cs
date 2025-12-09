using Models;
using Models.Dtos;
using Models.Interfaces;

namespace BuyActions.Services;

public class BuyService(DataContext dataContext, UserClientService userService, ShoppingCartClientService shoppingCartService) : IBuyService
{
    public Task<IEnumerable<BuyReportDto>> Get() => Task.FromResult<IEnumerable<BuyReportDto>>(dataContext.BuyReports);

    public Task<BuyReportDto> Get(Guid reportId)
    {
        var buyReport = dataContext.BuyReports.FirstOrDefault(x => x.Id == reportId);
        if (buyReport != null) return Task.FromResult(buyReport);
        throw new Exception("Buy report not found");
    }

    public async Task BuyCart(CartDto cartDto)
    {
        if (cartDto.User == null || cartDto.Place == null || cartDto.Orders.Count == 0) 
            throw new Exception("Cart is not full !!!");
        var moneyToSpend = cartDto.Orders.Where(x => x.OrderedProduct != null)
            .Sum(x => x.OrderedProduct!.Cost * x.Quantity);
        var isMoneyExist = cartDto.User.Wallet >= moneyToSpend;
        if (!isMoneyExist) throw new Exception("Not enough money on wallet !!!");
        if (!cartDto.IsConfirmed) throw new Exception("Cart is not confirmed !!!");

        var user = await userService.SpendMoney(cartDto.User.Id, moneyToSpend);
        cartDto.User = user;
        // something important and very slow
        await Task.Delay(7000);
        dataContext.BuyReports.Add(new BuyReportDto()
        {
            Id = Guid.NewGuid(),
            CartDto = cartDto,
            SaleDate = DateTime.Now
        });
        await shoppingCartService.MarkCartAsBought(cartDto.Id);
    }
}