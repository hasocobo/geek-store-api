using StoreApi.Entities;

namespace StoreApi.Features.Products
{
    public class ProductService : IProductService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IRepositoryManager repositoryManager, ILogger<ProductService> logger)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _repositoryManager.ProductRepository.GetAllProductsAsync();
        }

        public Task<Product> GetProductByIdAsync(Guid productId)
        {
            throw new NotImplementedException();
        }

        public Task CreateProductAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProductAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(Guid productId)
        {
            throw new NotImplementedException();
        }
    }
}