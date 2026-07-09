using Cart.Application.Interfaces;
using Cart.Application.Interfaces.Repositories;
using Cart.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Cart.Infrastructure.Data;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private ICartRepository _cartRepository;
    private IOrderRepository _orderRepository;
    private IPlaceRepository _placeRepository;
    private IBuyReportRepository _buyReportRepository;
    private IDbContextTransaction? _currentTransaction;
    public ICartRepository CartRepository => _cartRepository ??= new CartRepository(context);
    public IOrderRepository OrderRepository => _orderRepository ??= new OrderRepository(context);
    public IPlaceRepository PlaceRepository => _placeRepository ??= new PlaceRepository(context);
    public IBuyReportRepository BuyReportRepository => _buyReportRepository ??= new BuyReportRepository(context);

    public Task<int> CompleteAsync(CancellationToken cancellationToken = default) 
        => context.SaveChangesAsync(cancellationToken);

    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction != null) return;
        _currentTransaction = await context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await context.SaveChangesAsync();
            if (_currentTransaction != null)
            {
                await _currentTransaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            DisposeTransaction();
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.RollbackAsync();
            DisposeTransaction();
        }
    }

    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
    
    private void DisposeTransaction()
    {
        _currentTransaction?.Dispose();
        _currentTransaction = null;
    }
}