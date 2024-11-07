using StoreApi.Common.DataTransferObjects.Products;
using StoreApi.Entities;
using StoreApi.Entities.Exceptions;

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
            var products =
                await _repositoryManager.ProductRepository.GetProductsAsync();

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
            var product =
                await _repositoryManager.ProductRepository.GetProductByIdAsync(productId);
            if (product is null)
                throw new NotFoundException("Product", productId);
            
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
            if (!await _repositoryManager.CategoryRepository.CheckIfCategoryExists(categoryId))
                throw new NotFoundException("Category", categoryId);
            
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
            if (!await _repositoryManager.CategoryRepository.CheckIfCategoryExists(categoryId))
                throw new NotFoundException("Category", categoryId);
            
            _logger.LogInformation($"Creating new product");
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = productCreateDto.Name,
                Price = productCreateDto.Price,
                Description = productCreateDto.Description,
                CategoryId = categoryId
            };

            _logger.LogInformation($"Saving product with ID: {product.Id} to database");
            _repositoryManager.ProductRepository.CreateProduct(product);
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

        public async Task UpdateProductAsync(Guid productId, ProductUpdateDto productUpdateDto)
        {
            _logger.LogInformation($"Fetching product with ID: {productId} to update.");
            var productToUpdate = await
                _repositoryManager.ProductRepository.GetProductByIdAsync(productId);
            if (productToUpdate is null)
                throw new NotFoundException("Product", productId);
            
            _logger.LogInformation($"Updating product with ID: {productId}");
            if (productUpdateDto.Name != null) productToUpdate.Name = productUpdateDto.Name;
            if (productUpdateDto.Price.HasValue) productToUpdate.Price = productUpdateDto.Price.Value;
            if (productUpdateDto.Description != null) productToUpdate.Description = productUpdateDto.Description;
            if (productUpdateDto.Sku != null) productToUpdate.Sku = productUpdateDto.Sku;
            productToUpdate.CategoryId = productUpdateDto.CategoryId;
            
            _repositoryManager.ProductRepository.UpdateProduct(productToUpdate);
            
            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteProductAsync(Guid productId)
        {
            _logger.LogInformation($"Fetching product with ID: {productId} to delete");
            var productToDelete = 
                await _repositoryManager.ProductRepository.GetProductByIdAsync(productId);
            if (productToDelete is null)
                throw new NotFoundException("Product", productId);
            
            _logger.LogInformation($"Deleting product with ID: {productId}");
            _repositoryManager.ProductRepository.DeleteProduct(productToDelete);
            
            await _repositoryManager.SaveAsync();
        }
    }
}