using Models;

namespace BuyActions.Services;

public class BuyService(DataContext dataContext) : IBuyService
{
    public Task<IEnumerable<BuyReport>> Get() => Task.FromResult<IEnumerable<BuyReport>>(dataContext.BuyReports);

    public Task<BuyReport?> Get(Guid reportId) => 
        Task.FromResult(dataContext.BuyReports.FirstOrDefault(x => x.Id == reportId));

    public async Task BuyCart(Cart cart)
    {
        if (cart.User == null || cart.Place == null || cart.Orders.Count == 0) 
            throw new ArgumentNullException("Cart is not full !!!");
        var moneyToSpend = cart.Orders.Where(x => x.OrderedProduct != null)
            .Sum(x => x.OrderedProduct!.Cost * x.Quantity);
        var isMoneyExist = cart.User.Wallet >= moneyToSpend;
        if (!isMoneyExist) throw new Exception("Not enough money on wallet !!!");
        try
        {
            var user = await new UserClientService().SpendMoney(cart.User.Id, moneyToSpend);
            dataContext.BuyReports.Add(new BuyReport()
            {
                Id = Guid.NewGuid(),
                User = cart.User,
                Orders = cart.Orders,
                Place = cart.Place,
                SaleDate = DateTime.Now
            });
            await Task.Delay(5000);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}

public class UserClientService
{
    private const string BaseAddress = "https://localhost:7004/UserManipulations";
    public async Task<User?> GetUser(Guid userId)
    {
        var query = $"{BaseAddress}/{userId}";
        try
        {
            var result = await query.GetQuery<User>();
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<User?> SpendMoney(Guid userId, int money)
    {
        var query = $"{BaseAddress}/SpendMoney?userId={userId}&money={money}";
        return await query.PostQuery<User>();
    }
}