using Microsoft.EntityFrameworkCore;
using Models;
using Models.Dtos;
using Models.Interfaces;

namespace BuyActions.Services;

public class BuyService(
    DataContext dataContext,
    UserClientService userService,
    ShoppingCartClientService shoppingCartService,
    IProductCatalog productCatalogService) : IBuyService
{
    public async Task<IEnumerable<BuyReportDto>> Get()
    {
        var buyReportDtos = new List<BuyReportDto>();
        await foreach (var buyReport in dataContext.BuyReports)
        {
            var buyReportDto = await GetBuyReportDto(buyReport);
            buyReportDtos.Add(buyReportDto);
        }
        return await Task.FromResult<IEnumerable<BuyReportDto>>(buyReportDtos);
    }

    public async Task<BuyReportDto> Get(Guid reportId)
    {
        var buyReport = await dataContext.BuyReports.FirstOrDefaultAsync(x => x.Id == reportId);
        if (buyReport == null) throw new Exception("Buy report not found"); 
        var buyReportDto = await GetBuyReportDto(buyReport);
        return await Task.FromResult(buyReportDto);
    }
    
    public async Task<IEnumerable<BuyReportDto>> GetByUserId(Guid userId)
    {
        var buyReportDtos = new List<BuyReportDto>();
        await foreach (var buyReport in dataContext.BuyReports)
        {
            var buyReportDto = await GetBuyReportDto(buyReport);
            if (buyReportDto.BuyReportCart.User.Id == userId) buyReportDtos.Add(buyReportDto);
        }
        return await Task.FromResult<IEnumerable<BuyReportDto>>(buyReportDtos);
    }

    public async Task BuyCart(CartDto cartDto)
    {
        if (cartDto.PlaceId == null) throw new Exception("Can't buy cart, cart is not full !!!");

        var cartOrders = await shoppingCartService.GetCartOrders(cartDto.Id);
        if (!cartOrders?.Any() ?? true) throw new Exception("Can't buy cart, cart is not full !!!");
        
        // something important and very slow
        await Task.Delay(5000);
        
        var userDto = await userService.SpendMoney(cartDto.UserId, cartDto.AmountToPay);
        if (userDto == null) throw new Exception("Can't buy cart, user server is not working !!!");
        
        dataContext.BuyReports.Add(new BuyReport()
        {
            Id = Guid.NewGuid(),
            CartId = cartDto.Id,
            SaleDate = DateTime.Now
        });
        await dataContext.SaveChangesAsync();
        await shoppingCartService.MarkCartAsBought(cartDto.Id);
    }
    
    private async Task<BuyReportDto> GetBuyReportDto(BuyReport buyReport)
    {
        var cartDto = await shoppingCartService.GetCartById(buyReport.CartId);
        if (cartDto == null) throw new Exception("Cart not found !!!");
        
        var userDto = await userService.GetUser(cartDto.UserId);
        if (userDto == null) throw new Exception("User not found !!!");
        
        var placeDto = await shoppingCartService.GetPlace(cartDto.PlaceId);
        if (placeDto == null) throw new Exception("Place not found !!!");
        
        var cartOrders = await shoppingCartService.GetCartOrders(cartDto.Id);
        var buyReportOrderDtos = new List<BuyReportOrderDto>();
        if (cartOrders != null)
        {
            foreach (var cartOrder in cartOrders)
            {
                var product = await productCatalogService.Get(cartOrder.OrderedProductId);
                buyReportOrderDtos.Add(new BuyReportOrderDto()
                {
                    Id = cartOrder.Id,
                    Product = new ProductDto()
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Cost = product.Cost
                    },
                    Quantity = cartOrder.Quantity
                });
            }
        }
        
        var buyReportCart = new BuyReportCartDto()
        {
            Id = buyReport.CartId,
            User = userDto,
            Place = placeDto,
            AmountToPay = cartDto.AmountToPay,
            IsBought = cartDto.IsBought,
            IsConfirmed = cartDto.IsConfirmed,
            Orders = buyReportOrderDtos
        };
        return new BuyReportDto
        {
            Id = buyReport.Id,
            BuyReportCart = buyReportCart,
            SaleDate = buyReport.SaleDate
        };
    }
}