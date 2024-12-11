using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using StoreApi.Common.DataTransferObjects.Products;
using StoreApi.Common.QueryFeatures;
using StoreApi.Entities;
using StoreApi.Entities.Exceptions;
using StoreApi.Features;
using StoreApi.Features.Categories;
using StoreApi.Features.Products;
using Xunit;

namespace StoreApiTests.ProductTests;

public class ProductUnitTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IRepositoryManager> _repositoryManagerMock;
    private readonly IProductService _productService;

    public ProductUnitTests()
    {
        _repositoryManagerMock = new Mock<IRepositoryManager>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();

        _repositoryManagerMock.Setup(rm => rm.ProductRepository).Returns(_productRepositoryMock.Object);
        _repositoryManagerMock.Setup(rm => rm.CategoryRepository).Returns(_categoryRepositoryMock.Object);
        _repositoryManagerMock.Setup(rm => rm.SaveAsync()).Returns(Task.CompletedTask);

        _productService = new ProductService(_repositoryManagerMock.Object, NullLogger<ProductService>.Instance);
    }

    // filtering, sorting, searching are tested with integration testing
    [Fact]
    public async Task GetProductsAsync_ReturnsProducts()
    {
        // Arrange
        var queryParameters = new QueryParameters();
        var products = new List<Product>
        {
            new() { Id = Guid.NewGuid(), Name = "Product1", Price = 10 },
            new() { Id = Guid.NewGuid(), Name = "Product2", Price = 20 }
        };
        var metadata = new Metadata();

        _productRepositoryMock.Setup(pr => pr.GetProductsAsync(queryParameters))
            .ReturnsAsync((products, metadata));

        // Act
        var result = await _productService.GetProductsAsync(queryParameters);

        // Assert
        Assert.Equal(products.Count, result.Item1.Count());
        Assert.Equal(products.First().Name, result.Item1.First().Name);
    }

    [Fact]
    public async Task GetProductsByCategoryIdAsync_ValidId_ReturnsProducts()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var queryParameters = new QueryParameters();
        var products = new List<Product>
        {
            new() { Id = Guid.NewGuid(), Name = "Product1", Price = 10, CategoryId = categoryId },
            new() { Id = Guid.NewGuid(), Name = "Product2", Price = 20, CategoryId = categoryId }
        };
        var metadata = new Metadata();

        _categoryRepositoryMock.Setup(cr => cr.CheckIfCategoryExists(categoryId)).ReturnsAsync(true);
        _productRepositoryMock.Setup(pr => pr.GetProductsByCategoryIdAsync(categoryId, queryParameters))
            .ReturnsAsync((products, metadata));

        // Act
        var result = await _productService.GetProductsByCategoryIdAsync(categoryId, queryParameters);

        // Assert
        Assert.Equal(products.Count, result.Item1.Count());
        Assert.Equal(products.First().Name, result.Item1.First().Name);
    }

    [Fact]
    public async Task GetProductsByCategoryIdAsync_InvalidId_ThrowsNotFoundException()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        _categoryRepositoryMock.Setup(cr 
            => cr.CheckIfCategoryExists(categoryId)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _productService.GetProductsByCategoryIdAsync(categoryId, new QueryParameters()));
    }

    [Fact]
    public async Task GetProductByIdAsync_ValidId_ReturnsProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product { Id = productId, Name = "Product1", Price = 10 };

        _productRepositoryMock.Setup(pr => pr.GetProductByIdAsync(productId)).ReturnsAsync(product);

        // Act
        var result = await _productService.GetProductByIdAsync(productId);

        // Assert
        Assert.Equal(productId, result.Id);
        Assert.Equal(product.Name, result.Name);
        Assert.Equal(product.Price, result.Price);
    }

    [Fact]
    public async Task GetProductByIdAsync_InvalidId_ThrowsNotFoundException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _productRepositoryMock.Setup(pr 
            => pr.GetProductByIdAsync(productId)).ReturnsAsync(null as Product);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _productService.GetProductByIdAsync(productId));
    }

    [Fact]
    public async Task CreateProductAsync_ValidInput_ReturnsCreatedProduct()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var productCreateDto = new ProductCreateDto { Name = "New Product", Price = 100, Description = "Description" };
        var category = new Category { Id = categoryId, Name = "Category1" };

        _categoryRepositoryMock.Setup(cr 
            => cr.CheckIfCategoryExists(categoryId)).ReturnsAsync(true);
        _categoryRepositoryMock.Setup(cr 
            => cr.GetCategoryByIdAsync(categoryId)).ReturnsAsync(category);

        // Act
        var result = await _productService.CreateProductAsync(categoryId, productCreateDto);

        // Assert
        Assert.Equal(productCreateDto.Name, result.Name);
        Assert.Equal(productCreateDto.Price, result.Price);
    }

    [Fact]
    public async Task CreateProductAsync_InvalidInput_ThrowsNotFoundException()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var productCreateDto = new ProductCreateDto { Name = "New Product", Price = 100 };

        _categoryRepositoryMock.Setup(cr 
            => cr.CheckIfCategoryExists(categoryId)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _productService.CreateProductAsync(categoryId, productCreateDto));
    }

    [Fact]
    public async Task UpdateProductAsync_ValidInput_CallsRepository()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var productUpdateDto = new ProductUpdateDto { Name = "Updated Product", Price = 150 };
        var product = new Product { Id = productId, Name = "Old Product", Price = 100 };

        _productRepositoryMock.Setup(pr => pr.GetProductByIdAsync(productId)).ReturnsAsync(product);

        // Act
        await _productService.UpdateProductAsync(productId, productUpdateDto);

        // Assert
        _productRepositoryMock.Verify(pr =>
            pr.UpdateProduct(It.Is<Product>(p => p.Name == "Updated Product" && p.Price == 150)));
        _repositoryManagerMock.Verify(rm => 
            rm.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateProductAsync_InvalidInput_ThrowsNotFoundException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var productUpdateDto = new ProductUpdateDto { Price = 99 };

        _productRepositoryMock.Setup(pr
            => pr.GetProductByIdAsync(productId)).ReturnsAsync(null as Product);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _productService.UpdateProductAsync(productId, productUpdateDto));
    }

    [Fact]
    public async Task DeleteProductAsync_ValidInput_CallsRepository()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product { Id = productId, Name = "Product to Delete" };

        _productRepositoryMock.Setup(pr => pr.GetProductByIdAsync(productId)).ReturnsAsync(product);

        // Act
        await _productService.DeleteProductAsync(productId);

        // Assert
        _productRepositoryMock.Verify(pr
            => pr.DeleteProduct(product), Times.Once);
        _repositoryManagerMock.Verify(rm 
            => rm.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteProductAsync_InvalidInput_ThrowsNotFoundException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _productRepositoryMock.Setup(pr => pr.GetProductByIdAsync(productId)).ReturnsAsync(null as Product);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _productService.DeleteProductAsync(productId));
    }
}