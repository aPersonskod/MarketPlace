using Cart.Application.Dtos;
using Cart.Application.Interfaces;
using Cart.Application.Interfaces.Repositories;
using Cart.Application.Services;
using Model.Extensions;
using Moq;
using User.Application.Dto;
using User.Application.Interfaces;

namespace XUnitTestProject;

public static class Ext
{
    public const string ProductBaseAddress = "https://localhost:7001/api/product-service";
    public const string CartBaseAddress = "https://localhost:7002/api/cart-service";
    public const string BuyBaseAddress = "https://localhost:7003/api/buy-service";
    public const string UserBaseAddress = "https://localhost:7004/api/user-service";

    public const string ProductId = "35e52d12-62f8-4451-ab81-b549fa3f066b";
    public const string PlaceId = "f853bb36-6ad3-4d03-ad7e-9a3545d21429";
}

public class CartTests
{
    private Mock<IUnitOfWork> _moqCartUow;
    private Mock<ICartRepository> _moqCartRepo;
    private Mock<IOrderRepository> _moqOrderRepo;
    private Mock<IPlaceRepository> _moqPlaceRepo;
    private Mock<IBuyReportRepository> _moqBuyRepo;
    private Guid _userId = new Guid("51228188-871a-43a8-91d3-a2595fb42bed");
    private Guid _cartId = new Guid("e9a70b02-1889-4c50-8a39-46fb9202705c");
    private Guid _orderId = new Guid("971d0e07-18c6-4241-b8a4-d2d369f7d54d");
    private Guid _placeId = new Guid("f853bb36-6ad3-4d03-ad7e-9a3545d21429");

    public CartTests()
    {
        _moqCartUow = new Mock<IUnitOfWork>();
        _moqCartRepo = new Mock<ICartRepository>();
        _moqOrderRepo = new Mock<IOrderRepository>();
        _moqPlaceRepo = new Mock<IPlaceRepository>();
        _moqBuyRepo = new Mock<IBuyReportRepository>();
    }
    
    [Fact]
    public async Task Test_ConfirmCart()
    {
        var loginApi = $"{Ext.UserBaseAddress}/login";
        var credentials = new UserCredentialsDto()
        {
            Email = "test@gmail.com",
            Password = "test_test"
        };
        var token = await loginApi.PostQuery<string, UserCredentialsDto>(credentials);
        Assert.NotNull(token);
        
        var addMoneyApi = $"{Ext.UserBaseAddress}/top-up-money";
        var moneyDto = new MoneyDto()
        {
            Money = 100
        };
        var user = await addMoneyApi.PatchQuery<UserDto, MoneyDto>(moneyDto, token);
        Assert.NotNull(user);
        
        var getCartApi = $"{Ext.CartBaseAddress}/get-cart";
        var cartDto = await getCartApi.GetQuery<CartDto>(token);
        Assert.NotNull(cartDto);
        
        var addOrderApi = $"{Ext.CartBaseAddress}/add-order";
        var createOrderDto = new CreateOrderDto()
        {
            CartId = cartDto.Id,
            OrderedProductId = new Guid(Ext.ProductId), // носки по 50
            Quantity = 2
        };
        var orderDto = await addOrderApi.PostQuery<OrderDto, CreateOrderDto>(createOrderDto, token);
        Assert.NotNull(orderDto);

        var confirmCartApi = $"{Ext.CartBaseAddress}/confirm-cart?placeId={Ext.PlaceId}";
        var confirmedCartDto = await confirmCartApi.PatchQuery<CartDto>(token);
        
        Assert.NotNull(confirmedCartDto);
        Assert.True(confirmedCartDto.IsConfirmed);
    }

    [Fact]
    public async Task Test_ConfirmCartMoq()
    {
        var requestCart = new Model.Cart()
        {
            Id = _cartId,
            UserId = _userId,
            PlaceId = null,
            AmountToPay = 100,
            IsConfirmed = false,
            IsBought = false
        };
        _moqCartRepo.Setup(x => x.GetUnverifiedCartByUserIdAsync(_userId)).ReturnsAsync(requestCart);
        _moqCartRepo.Setup(x => x.AddPlaceToCart(_userId, _placeId)).ReturnsAsync(() =>
        {
            requestCart.PlaceId = _placeId;
            return requestCart;
        });
        _moqCartRepo.Setup(x => x.ConfirmCartAsync(_userId)).ReturnsAsync(() =>
        {
            requestCart.IsConfirmed = true;
            return requestCart;
        });
        _moqPlaceRepo.Setup(x => x.GetPlaceByIdAsync(_placeId)).ReturnsAsync(new Model.Place()
        {
            Id = _placeId,
            Address = "ул. Пушкина, дом колотушкина",
            WorkingTime = "10:00 - 11:00"
        });
        _moqCartUow.Setup(x => x.CartRepository).Returns(_moqCartRepo.Object);
        _moqCartUow.Setup(x => x.OrderRepository).Returns(_moqOrderRepo.Object);
        _moqCartUow.Setup(x => x.PlaceRepository).Returns(_moqPlaceRepo.Object);
        _moqCartUow.Setup(x => x.BuyReportRepository).Returns(_moqBuyRepo.Object);
        var cartService = new CartService(_moqCartUow.Object);

        var cart = await cartService.ConfirmCartAsync(_userId, _placeId);
        Assert.Equal(_placeId, cart.PlaceId);
        Assert.Equal(100, cart.AmountToPay);
        Assert.True(cart.IsConfirmed);
        Assert.False(cart.IsBought);
    }
}