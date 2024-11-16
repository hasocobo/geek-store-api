using StoreApi.Common.QueryFeatures;
using StoreApi.Entities;

namespace StoreApi.Features.Products
{
    public interface IProductRepository
    {
        Task<bool> CheckIfProductExists(Guid productId);
        Task<(IEnumerable<Product>, Metadata)> GetProductsAsync(QueryParameters queryParameters);
        Task<Product?> GetProductByIdAsync(Guid productId);

        Task<(IEnumerable<Product>, Metadata)> GetProductsByCategoryIdAsync(Guid categoryId,
            QueryParameters queryParameters);

        void CreateProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
    }
}