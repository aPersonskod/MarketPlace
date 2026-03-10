using BuyActions.Queries;
using MediatR;
using Models.Dtos;

namespace BuyActions.Handlers;

public class GetBuyReportQueryHandler(IMediator mediator) : IRequestHandler<GetBuyReportQuery, BuyReportDto?>
{
    public async Task<BuyReportDto?> Handle(GetBuyReportQuery request, CancellationToken cancellationToken)
    {
        if(cancellationToken.IsCancellationRequested)
            return await Task.FromResult<BuyReportDto?>(null);

        // todo validation
        var cartDto = await mediator.Send(new GetCartQuery(request.Report.CartId), cancellationToken);
        if (cartDto == null) throw new Exception("Cart not found !!!");
        
        var userDto = await mediator.Send(new GetUserQuery(request.AccessToken), cancellationToken);
        if (userDto == null) throw new Exception("User not found !!!");
        
        if (cartDto.PlaceId == null) throw new Exception("Place not found !!!");
        var placeDto = await mediator.Send(new GetCartPlaceQuery((Guid)cartDto.PlaceId), cancellationToken);
        if (placeDto == null) throw new Exception("Place not found !!!");
        
        var cartOrders = await mediator.Send(new GetCartOrdersQuery(cartDto.Id), cancellationToken);
        var buyReportOrderDtos = new List<BuyReportOrderDto>();
        if (cartOrders != null)
        {
            foreach (var cartOrder in cartOrders)
            {
                var product = await mediator.Send(new GetProductQuery(cartOrder.OrderedProductId), cancellationToken);
                buyReportOrderDtos.Add(new BuyReportOrderDto()
                {
                    Id = cartOrder.Id,
                    Product = new ProductDto()
                    {
                        Id = product!.Id,
                        Name = product.Name,
                        Cost = product.Cost
                    },
                    Quantity = cartOrder.Quantity
                });
            }
        }
        
        var buyReportCart = new BuyReportCartDto()
        {
            Id = request.Report.CartId,
            User = userDto,
            Place = placeDto,
            AmountToPay = cartDto.AmountToPay,
            IsBought = cartDto.IsBought,
            IsConfirmed = cartDto.IsConfirmed,
            Orders = buyReportOrderDtos
        };
        return new BuyReportDto
        {
            Id = request.Report.Id,
            BuyReportCart = buyReportCart,
            SaleDate = request.Report.SaleDate
        };
    }
}