namespace Models;

public interface IBuyService
{
    Task<IEnumerable<BuyReport>> Get();
    Task<BuyReport?> Get(Guid reportId);
    Task BuyCart(Cart cart);
}