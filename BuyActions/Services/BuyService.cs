using Models;
using Models.Interfaces;

namespace BuyActions.Services;

public class BuyService(DataContext dataContext, UserClientService userService, ShoppingCartClientService shoppingCartService) : IBuyService
{
    public Task<IEnumerable<BuyReport>> Get() => Task.FromResult<IEnumerable<BuyReport>>(dataContext.BuyReports);

    public Task<BuyReport> Get(Guid reportId)
    {
        var buyReport = dataContext.BuyReports.FirstOrDefault(x => x.Id == reportId);
        if (buyReport != null) return Task.FromResult(buyReport);
        throw new Exception("Buy report not found");
    }

    public async Task BuyCart(Cart cart)
    {
        if (cart.User == null || cart.Place == null || cart.Orders.Count == 0) 
            throw new Exception("Cart is not full !!!");
        var moneyToSpend = cart.Orders.Where(x => x.OrderedProduct != null)
            .Sum(x => x.OrderedProduct!.Cost * x.Quantity);
        var isMoneyExist = cart.User.Wallet >= moneyToSpend;
        if (!isMoneyExist) throw new Exception("Not enough money on wallet !!!");
        if (!cart.IsConfirmed) throw new Exception("Cart is not confirmed !!!");

        var user = await userService.SpendMoney(cart.User.Id, moneyToSpend);
        cart.User = user;
        // something important and very slow
        await Task.Delay(7000);
        dataContext.BuyReports.Add(new BuyReport()
        {
            Id = Guid.NewGuid(),
            Cart = cart,
            SaleDate = DateTime.Now
        });
        await shoppingCartService.MarkCartAsBought(cart.Id);
    }
}