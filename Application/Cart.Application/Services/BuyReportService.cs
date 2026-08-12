using Cart.Application.Dtos;
using Cart.Application.Interfaces;
using Cart.Application.Interfaces.Services;
using Model.SharedExceptions;

namespace Cart.Application.Services;

public class BuyReportService(
    IUnitOfWork unitOfWork,
    ICartService cartService,
    IOrderService orderService,
    IPlaceService placeService,
    IProductService productService) : IBuyReportService
{
    private List<CartForReportDto> _cartReportDtos = [];
    private readonly List<CartForReportDetailedDto> _detailedCarts = [];

    [Obsolete]
    public async Task<IEnumerable<CartForReportDetailedDto>> GetCartsForReportAsync(Guid userId)
    {
        var cartForReportDtos = await unitOfWork.BuyReportRepository.GetCartsForReportAsync(userId);
        if (cartForReportDtos is null) throw new NoContentException("No reports");
        _cartReportDtos = cartForReportDtos.ToList();
        if (_cartReportDtos.Count == 0) throw new NoContentException("No reports");
        var groupedReportsDto = _cartReportDtos.GroupBy(x => x.Id);
        foreach (var forReportDtos in groupedReportsDto)
        {
            var detailedCartForReport = new CartForReportDetailedDto() { CartId = forReportDtos.Key };
            foreach (var cartForReportDto in forReportDtos)
            {
                var productDto = await productService.GetProductByIdAsync(cartForReportDto.OrderedProductId);
                detailedCartForReport.UserId = cartForReportDto.UserId;
                detailedCartForReport.Address = cartForReportDto.Address;
                detailedCartForReport.Orders.Add(new OrderForReportDto()
                {
                    Id = cartForReportDto.OrderId,
                    Product = new ProductDto()
                    {
                        Id = productDto.Id,
                        Name = productDto.Name,
                        Cost = productDto.Cost
                    },
                    Quantity = cartForReportDto.Quantity
                });
                detailedCartForReport.AmountToPay = cartForReportDto.AmountToPay;
            }

            _detailedCarts.Add(detailedCartForReport);
        }

        return _detailedCarts;
    }

    public async Task<CartForReportDetailedDto?> GetCartForReportAsync(Guid cartId)
    {
        var cart = await cartService.GetCartByIdAsync(cartId);
        if (cart is null) throw new NotFoundException("Cart not found");
        if (cart.PlaceId == Guid.Empty) throw new NotFoundException("Cart is not bought");
        var place = await placeService.GetPlaceAsync((Guid)cart.PlaceId!);
        var orders = await orderService.GetAllOrdersAsync(cart.Id);
        var ordersForReport = new List<OrderForReportDto>();
        foreach (var order in orders)
        {
            var product = await productService.GetProductByIdAsync(order.OrderedProductId);
            ordersForReport.Add(new OrderForReportDto()
            {
                Id = order.OrderedProductId,
                Product = product,
                Quantity = order.Quantity
            });
        }
        return new CartForReportDetailedDto()
        {
            CartId = cartId,
            UserId = cart.UserId,
            Address = place.Address,
            AmountToPay = cart.AmountToPay,
            Orders = ordersForReport
        }; 
    }
}