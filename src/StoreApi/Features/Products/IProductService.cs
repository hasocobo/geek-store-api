using StoreApi.Common.DataTransferObjects.Products;
using StoreApi.Common.QueryFeatures;
using StoreApi.Entities;

namespace StoreApi.Features.Products
{
    public interface IProductService
    {
        Task<(IEnumerable<ProductReadDto>, Metadata)> GetProductsAsync(QueryParameters queryParameters);
        Task<IEnumerable<ProductReadDto>> GetProductsByCategoryIdAsync(Guid categoryId);
        Task<ProductReadDto> GetProductByIdAsync(Guid productId);
        Task<ProductReadDto> CreateProductAsync(Guid categoryId, ProductCreateDto productCreateDto);
        Task UpdateProductAsync(Guid id, ProductUpdateDto productUpdateDto);
        Task DeleteProductAsync(Guid productId);
    }
}
