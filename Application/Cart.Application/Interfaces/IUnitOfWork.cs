using Cart.Application.Interfaces.Repositories;

namespace Cart.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICartRepository CartRepository { get; }
    IOrderRepository OrderRepository { get; }
    IPlaceRepository PlaceRepository { get; }
    IBuyReportRepository BuyReportRepository { get; }
    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
}