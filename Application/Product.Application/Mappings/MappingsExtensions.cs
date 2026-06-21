using Model.SharedExceptions;
using Product.Application.Dtos;

namespace Product.Application.Mappings;

public static class MappingsExtensions
{
    public static ProductDto ToDto(this Model.Product product)
    {
        if (product == null) throw new NotFoundException("User not found");
        return new ProductDto()
        {
            Id = product.Id,
            Name = product.Name,
            Cost = product.Cost
        };
    }
}