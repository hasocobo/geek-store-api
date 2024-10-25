using StoreApi.Entities;

namespace StoreApi.Features.Products
{
    public interface IProductService
    {
        
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(Guid productId);
        Task CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Guid productId);
    }
}
