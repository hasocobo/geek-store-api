using StoreApi.Common.DataTransferObjects.Products;
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

        public async Task<IEnumerable<ProductReadDto>> GetProductsAsync()
        {
            _logger.LogInformation("Getting all products");
            var products = await _repositoryManager.ProductRepository.GetProductsAsync();

            _logger.LogInformation("Returning all products");
            var productsToReturn = products.Select(p => new ProductReadDto
            (
                p.Id,
                p.Name ?? string.Empty,
                p.Price,
                p.Description,
                p.Category?.Name ?? string.Empty
            ));

            return productsToReturn;
        }

        public async Task<ProductReadDto> GetProductByIdAsync(Guid productId)
        {
            _logger.LogInformation($"Getting product with id {productId}");
            var product = await _repositoryManager.ProductRepository.GetProductByIdAsync(productId);

            _logger.LogInformation($"Returning product with id {productId}");
            var productToReturn = new ProductReadDto
            (
                product.Id,
                product.Name ?? string.Empty,
                product.Price,
                product.Description,
                product.Category?.Name ?? string.Empty
            );

            return productToReturn;
        }

        public async Task<IEnumerable<ProductReadDto>> GetProductsByCategoryIdAsync(Guid categoryId)
        {
            _logger.LogInformation($"Getting all products with category ID: {categoryId}");
            var products = await
                _repositoryManager.ProductRepository.GetProductsByCategoryIdAsync(categoryId);

            _logger.LogInformation($"Returning all products with category ID: {categoryId}");
            var productsToReturn = products.Select(p => new ProductReadDto
            (
                p.Id,
                p.Name ?? string.Empty,
                p.Price,
                p.Description,
                p.Category?.Name ?? string.Empty
            ));

            return productsToReturn;
        }

        public async Task<ProductReadDto> CreateProductAsync(Guid categoryId, ProductCreateDto productCreateDto)
        {
            _logger.LogInformation($"Creating new product");
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = productCreateDto.Name,
                Price = productCreateDto.Price,
                Description = productCreateDto.Description
            };

            _logger.LogInformation($"Saving product with ID: {product.Id} to database");
            _repositoryManager.ProductRepository.CreateProduct(categoryId, product);
            await _repositoryManager.SaveAsync();


            _logger.LogInformation($"Fetching category with ID: {categoryId}");
            var category = await
                _repositoryManager.CategoryRepository.GetCategoryByIdAsync(categoryId);

            _logger.LogInformation($"Returning created product's details");
            var productToReturn = new ProductReadDto
            (
                Id: product.Id,
                Name: product.Name ?? string.Empty,
                Price: product.Price,
                Description: product.Description,
                CategoryName: category.Name ?? string.Empty
            );

            return productToReturn;
        }

        public Task UpdateProductAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteProductAsync(Guid productId)
        {
            _logger.LogInformation($"Fetching product with ID: {productId} to delete");
            var productToDelete = await _repositoryManager.ProductRepository.GetProductByIdAsync(productId);

            _logger.LogInformation($"Deleting product with ID: {productId}");
            _repositoryManager.ProductRepository.DeleteProduct(productToDelete);
            await _repositoryManager.SaveAsync();
        }
    }
}