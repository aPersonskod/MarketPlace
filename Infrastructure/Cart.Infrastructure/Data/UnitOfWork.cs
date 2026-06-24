using Cart.Application.Interfaces;
using Cart.Application.Interfaces.Repositories;
using Cart.Infrastructure.Repositories;

namespace Cart.Infrastructure.Data;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private ICartRepository _cartRepository;
    private IOrderRepository _orderRepository;
    private IPlaceRepository _placeRepository;
    private IBuyReportRepository _buyReportRepository;
    public ICartRepository CartRepository => _cartRepository ??= new CartRepository(context);
    public IOrderRepository OrderRepository => _orderRepository ??= new OrderRepository(context);
    public IPlaceRepository PlaceRepository => _placeRepository ??= new PlaceRepository(context);
    public IBuyReportRepository BuyReportRepository => _buyReportRepository ??= new BuyReportRepository(context);

    public Task<int> CompleteAsync(CancellationToken cancellationToken = default) 
        => context.SaveChangesAsync(cancellationToken);

    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
}