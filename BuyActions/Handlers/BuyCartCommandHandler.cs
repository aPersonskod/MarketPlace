using BuyActions.Commands;
using BuyActions.Queries;
using MediatR;
using Models;

namespace BuyActions.Handlers;

public class BuyCartCommandHandler(DataContext dataContext, IMediator mediator) : IRequestHandler<BuyCartCommand, bool>
{
    public async Task<bool> Handle(BuyCartCommand request, CancellationToken cancellationToken)
    {
        if (request.CartDto.PlaceId == null) throw new Exception("Can't buy cart, cart is not full !!!");

        var cartOrders = await mediator.Send(new GetCartOrdersQuery(request.CartDto.Id), cancellationToken);
        if (!cartOrders?.Any() ?? true) throw new Exception("Can't buy cart, cart is not full !!!");
        
        // something important and very slow
        await Task.Delay(5000, cancellationToken);
        
        var userDto = await mediator.Send(
                new UserSpendMoneyCommand(request.CartDto.AmountToPay, request.AccessToken), cancellationToken);
        if (userDto == null) throw new Exception("Can't buy cart, user server is not working !!!");
        
        dataContext.BuyReports.Add(new BuyReport()
        {
            Id = Guid.NewGuid(),
            CartId = request.CartDto.Id,
            SaleDate = DateTime.Now
        });
        await dataContext.SaveChangesAsync(cancellationToken);
        return await mediator.Send(new CartMarkAsBoughtCommand(request.CartDto.Id), cancellationToken);
    }
}