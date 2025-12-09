using Models.Dtos;

namespace Models.Interfaces;

public interface IBuyService
{
    Task<IEnumerable<BuyReportDto>> Get();
    Task<BuyReportDto> Get(Guid reportId);
    Task BuyCart(CartDto cartDto);
}