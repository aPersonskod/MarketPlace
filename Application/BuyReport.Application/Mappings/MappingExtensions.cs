using BuyReport.Application.Dtos;
using Model.SharedExceptions;

namespace BuyReport.Application.Mappings;

public static class MappingExtensions
{
    public static BuyReportDto ToDto(this Model.BuyReport buyReport)
    {
        if (buyReport == null) throw new NotFoundException("BuyReport not found");
        return new BuyReportDto()
        {
            Id = buyReport.Id,
            CartId = buyReport.CartId,
            SaleDate = buyReport.SaleDate
        };
    }

    public static DetailedCartForReportDto ToDetailedDto(this CartForReportDto cartForReportDto, UserDto? userDto)
    {
        if (cartForReportDto == null) throw new NotFoundException("Cart for report not found");
        if (userDto == null) throw new NotFoundException("User not found");
        return new DetailedCartForReportDto()
        {
            User = userDto,
            CartId = cartForReportDto.CartId,
            Address = cartForReportDto.Address,
            Products = cartForReportDto.Products,
            Quantity = cartForReportDto.Quantity,
            AmountToPay = cartForReportDto.AmountToPay
        };
    }
}