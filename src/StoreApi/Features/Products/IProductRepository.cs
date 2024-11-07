using StoreApi.Common.QueryParameters;
using StoreApi.Entities;

namespace StoreApi.Features.Products
{
    public interface IProductRepository
    {
        Task<bool> CheckIfProductExists(Guid productId);
        Task<IEnumerable<Product>> GetProductsAsync(QueryParameters queryParameters); 
        Task<Product?> GetProductByIdAsync(Guid productId);
        Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(Guid categoryId);
        void CreateProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
    }
}
