using StoreApi.Common.DataTransferObjects.Products;
using StoreApi.Entities;

namespace StoreApi.Features.Products
{
    public interface IProductService
    {
        Task<IEnumerable<ProductReadDto>> GetProductsAsync();
        Task<IEnumerable<ProductReadDto>> GetProductsByCategoryIdAsync(Guid categoryId);
        Task<ProductReadDto> GetProductByIdAsync(Guid productId);
        Task<ProductReadDto> CreateProductAsync(Guid categoryId, ProductCreateDto productCreateDto);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Guid productId);
    }
}
