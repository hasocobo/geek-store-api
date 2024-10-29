using StoreApi.Entities;

namespace StoreApi.Features.Products
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(); 
        Task<Product> GetProductByIdAsync(Guid productId);
        void CreateProduct(Product product);
    }
}
