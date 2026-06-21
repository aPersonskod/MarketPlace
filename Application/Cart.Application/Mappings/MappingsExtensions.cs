using Cart.Application.Dtos;
using Model;
using Model.SharedExceptions;

namespace Cart.Application.Mappings;

public static class MappingsExtensions
{
    public static CartDto ToDto(this Model.Cart? cart)
    {
        if (cart == null) throw new NotFoundException("Cart not found");
        return new CartDto()
        {
            Id = cart.Id,
            UserId = cart.UserId,
            PlaceId = cart.PlaceId,
            AmountToPay = cart.AmountToPay,
            IsConfirmed = cart.IsConfirmed,
            IsBought = cart.IsBought
        };
    }

    public static OrderDto ToDto(this Model.Order? order)
    {
        if (order == null) throw new NotFoundException("Order not found");
        return new OrderDto()
        {
            Id = order.Id,
            CartId = order.CartId,
            OrderedProductId = order.OrderedProductId,
            Quantity = order.Quantity
        };
    }

    public static PlaceDto ToDto(this Model.Place? place)
    {
        if (place == null) throw new NotFoundException("Place not found");
        return new PlaceDto()
        {
            Id = place.Id,
            Address = place.Address,
            WorkingTime = place.WorkingTime
        };
    }
}