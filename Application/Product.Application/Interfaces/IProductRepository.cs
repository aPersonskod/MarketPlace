namespace Product.Application.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Model.Product>> GetProducts();
    Task<Model.Product?> GetProductById(Guid productId);
}