using Models;
using Models.Interfaces;

namespace BuyActions.Services;

public class BuyService(DataContext dataContext) : IBuyService
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

        var user = await new UserClientService().SpendMoney(cart.User.Id, moneyToSpend);
        if (user != null) cart.User = user;
        dataContext.BuyReports.Add(new BuyReport()
        {
            Id = Guid.NewGuid(),
            Cart = cart,
            SaleDate = DateTime.Now
        });
        await Task.Delay(5000);
    }
}

public class UserClientService
{
    private const string BaseAddress = "https://localhost:7004/UserManipulations";
    public async Task<User> GetUser(Guid userId)
    {
        var query = $"{BaseAddress}/{userId}";
        return (await query.GetQuery<User>())!;
    }

    public async Task<User> SpendMoney(Guid userId, int money)
    {
        var query = $"{BaseAddress}/SpendMoney?userId={userId}&money={money}";
        return (await query.PostQuery<User>())!;
    }
}