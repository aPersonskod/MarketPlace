using Cart.Application.Dtos;
using Cart.Application.Interfaces;
using Cart.Application.Interfaces.Services;
using Model.SharedExceptions;
using Product.Application.Dtos;

namespace Cart.Application.Services;

public class BuyReportService(IUnitOfWork unitOfWork, IProductService productService) : IBuyReportService
{
    private List<CartForReportDto> _cartReportDtos = [];
    private readonly List<CartForReportDetailedDto> _detailedCarts = [];

    public async Task<IEnumerable<CartForReportDetailedDto>> GetCartsForReportAsync(Guid userId)
    {
        var cartForReportDtos = await unitOfWork.BuyReportRepository.GetCartForReportAsync(userId);
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
                detailedCartForReport.Products.Add(new ProductDto()
                {
                    Id = productDto.Id,
                    Name = productDto.Name,
                    Cost = productDto.Cost
                });
                detailedCartForReport.Quantity = cartForReportDto.Quantity;
                detailedCartForReport.AmountToPay = cartForReportDto.AmountToPay;
            }

            _detailedCarts.Add(detailedCartForReport);
        }

        return _detailedCarts;
    }
}