using StoreApi.Entities;

namespace StoreApi.Features.Products
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync(); 
        Task<Product> GetProductByIdAsync(Guid productId);
        Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(Guid categoryId);
        void CreateProduct(Guid categoryId, Product product);
        
        void DeleteProduct(Product product);
    }
}
