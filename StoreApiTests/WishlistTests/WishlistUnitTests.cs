using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using StoreApi.Common.DataTransferObjects.Wishlists;
using StoreApi.Entities;
using StoreApi.Entities.Exceptions;
using StoreApi.Features;
using StoreApi.Features.Customers;
using StoreApi.Features.Products;
using StoreApi.Features.Wishlists;

namespace StoreApiTests.WishlistTests;

public class WishlistUnitTests
{
    private readonly Mock<IRepositoryManager> _repositoryManagerMock;
    private readonly Mock<IWishlistRepository> _wishlistRepositoryMock;
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly IWishlistService _wishlistService;

    public WishlistUnitTests()
    {
        _wishlistRepositoryMock = new Mock<IWishlistRepository>();
        _repositoryManagerMock = new Mock<IRepositoryManager>();
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _productRepositoryMock = new Mock<IProductRepository>();

        _repositoryManagerMock.Setup(rm
            => rm.WishlistRepository).Returns(_wishlistRepositoryMock.Object);

        _repositoryManagerMock.Setup(rm
            => rm.CustomerRepository).Returns(_customerRepositoryMock.Object);

        _repositoryManagerMock.Setup(rm
            => rm.ProductRepository).Returns(_productRepositoryMock.Object);

        _repositoryManagerMock.Setup(rm
            => rm.SaveAsync()).Returns(Task.CompletedTask);

        _wishlistService = new WishlistService(_repositoryManagerMock.Object, NullLogger<WishlistService>.Instance);
    }

    [Fact]
    public async Task GetWishlistItemByIdAsync_ValidId_ReturnsWishlistItem()
    {
        // Arrange
        var wishlistItemId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var expectedWishlistItem = new WishlistReadDto(Id: wishlistItemId, ProductId: productId,
            ProductName: "Test Product", ProductDescription: "Test Product Description", CustomerId: customerId);

        _wishlistRepositoryMock.Setup(repo => repo.GetWishlistByIdAsync(wishlistItemId))
            .ReturnsAsync(new Wishlist
            {
                Id = wishlistItemId,
                ProductId = productId,
                Product = new Product
                    { Id = productId, Name = "Test Product", Description = "Test Product Description" },
                CustomerId = customerId
            });

        // Act
        var result = await _wishlistService.GetWishlistItemByIdAsync(wishlistItemId);

        // Assert
        Assert.Equal(expectedWishlistItem, result);
    }

    [Fact]
    public async Task GetWishlistItemByIdAsync_InvalidId_ThrowsNotFoundException()
    {
        // Arrange
        var wishlistItemId = Guid.NewGuid();
        _wishlistRepositoryMock.Setup(repo
            => repo.GetWishlistByIdAsync(wishlistItemId)).ReturnsAsync(null as Wishlist);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _wishlistService.GetWishlistItemByIdAsync(wishlistItemId));
    }

    [Fact]
    public async Task GetWishlists_ReturnsWishlists()
    {
        // Arrange
        var customerIds = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        var productIds = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        var wishlist = new List<Wishlist>()
        {
            new Wishlist()
            {
                Id = Guid.NewGuid(), CustomerId = customerIds.ElementAt(0), ProductId = productIds.ElementAt(0),
                Product = new Product()
                {
                    Id = productIds.ElementAt(0), Name = "Test Product 1 ", Description = "Test Product Description 1"
                }
            },
            new Wishlist()
            {
                Id = Guid.NewGuid(), CustomerId = customerIds.ElementAt(1), ProductId = productIds.ElementAt(1),
                Product = new Product()
                {
                    Id = productIds.ElementAt(1), Name = "Test Product 2 ", Description = "Test Product Description 2"
                }
            },
            new Wishlist()
            {
                Id = Guid.NewGuid(), CustomerId = customerIds.ElementAt(2), ProductId = productIds.ElementAt(1),
                Product = new Product()
                {
                    Id = productIds.ElementAt(1), Name = "Test Product 2 ", Description = "Test Product Description 2"
                }
            },
            new Wishlist()
            {
                Id = Guid.NewGuid(), CustomerId = customerIds.ElementAt(2), ProductId = productIds.ElementAt(2),
                Product = new Product()
                {
                    Id = productIds.ElementAt(2), Name = "Test Product 3 ", Description = "Test Product Description 3"
                }
            },
        };

        _wishlistRepositoryMock.Setup(repo
            => repo.GetWishlistsAsync()).ReturnsAsync(wishlist);

        // Act
        var result = await _wishlistService.GetWishlistsAsync();

        // Assert
        Assert.Equal(wishlist.Count, result.Count());
    }

    [Fact]
    public async Task GetWishlistByCustomerIdAsync_ValidId_ReturnsWishlist()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var productIds = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        var wishlist = new List<Wishlist>()
        {
            new Wishlist()
            {
                Id = Guid.NewGuid(), CustomerId = customerId, ProductId = productIds.ElementAt(0),
                Product = new Product()
                {
                    Id = productIds.ElementAt(0), Name = "Test Product 1 ", Description = "Test Product Description 1"
                }
            },
            new Wishlist()
            {
                Id = Guid.NewGuid(), CustomerId = customerId, ProductId = productIds.ElementAt(1),
                Product = new Product()
                {
                    Id = productIds.ElementAt(1), Name = "Test Product 2 ", Description = "Test Product Description 2"
                }
            },
            new Wishlist()
            {
                Id = Guid.NewGuid(), CustomerId = customerId, ProductId = productIds.ElementAt(2),
                Product = new Product()
                {
                    Id = productIds.ElementAt(2), Name = "Test Product 3 ", Description = "Test Product Description 3"
                }
            },
        };

        _customerRepositoryMock.Setup(repo
            => repo.CheckIfCustomerExists(customerId)).ReturnsAsync(true);

        _wishlistRepositoryMock.Setup(repo
            => repo.GetWishlistByCustomerIdAsync(customerId)).ReturnsAsync(wishlist);

        // Act
        var result = await _wishlistService.GetWishlistByCustomerIdAsync(customerId);

        // Assert
        Assert.Equal(wishlist.Count, result.Count());
    }

    [Fact]
    public async Task GetWishlistByCustomerIdAsync_InvalidId_ThrowsNotFoundException()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        _customerRepositoryMock.Setup(repo
            => repo.CheckIfCustomerExists(customerId)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _wishlistService.GetWishlistByCustomerIdAsync(customerId));
    }

    [Fact]
    public async Task CreateWishlistItemForCustomerAsync_ValidInput_ReturnsCreatedWishlistItem()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var product = new Product() { Id = productId, Name = "Test", Description = "Test" };

        var wishlistToCreate = new WishlistCreateDto(ProductId: productId);
        var createdWishlist = new WishlistReadDto(
            Id: Guid.NewGuid(),
            ProductId: productId,
            CustomerId: customerId,
            ProductDescription: product.Description,
            ProductName: product.Name
        );

        _customerRepositoryMock.Setup(r
            => r.CheckIfCustomerExists(customerId)).ReturnsAsync(true);
        _productRepositoryMock.Setup(r
            => r.CheckIfProductExists(productId)).ReturnsAsync(true);

        _productRepositoryMock.Setup(r
            => r.GetProductByIdAsync(productId)).ReturnsAsync(product);

        _wishlistRepositoryMock.Setup(r => r.CreateWishlist(It.IsAny<Wishlist>()));

        // Act
        var result = await _wishlistService.CreateWishlistItemForCustomerAsync(customerId, wishlistToCreate);

        // Assert
        Assert.Equal(result.CustomerId, createdWishlist.CustomerId);
        Assert.Equal(result.ProductId, createdWishlist.ProductId);
        Assert.Equal(result.ProductDescription, createdWishlist.ProductDescription);
        Assert.Equal(result.ProductName, createdWishlist.ProductName);
        _wishlistRepositoryMock.Verify(r => r.CreateWishlist(It.IsAny<Wishlist>()), Times.Once());
        _repositoryManagerMock.Verify(rm => rm.SaveAsync(), Times.Once());
    }

    [Fact]
    public async Task CreateWishlistItemForCustomerAsync_InvalidCustomerId_ThrowsNotFoundException()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var wishlistToCreate = new WishlistCreateDto(ProductId: productId);
        _customerRepositoryMock.Setup(r
            => r.CheckIfCustomerExists(customerId)).ReturnsAsync(false);
        _productRepositoryMock.Setup(r
            => r.CheckIfProductExists(productId)).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _wishlistService.CreateWishlistItemForCustomerAsync(customerId, wishlistToCreate));
    }

    [Fact]
    public async Task CreateWishlistItemForCustomerAsync_InvalidProductId_ThrowsNotFoundException()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var wishlistToCreate = new WishlistCreateDto(ProductId: productId);
        _customerRepositoryMock.Setup(r
            => r.CheckIfCustomerExists(customerId)).ReturnsAsync(true);
        _productRepositoryMock.Setup(r
            => r.CheckIfProductExists(productId)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _wishlistService.CreateWishlistItemForCustomerAsync(customerId, wishlistToCreate));
    }

    [Fact]
    public async Task DeleteWishlistItemByIdAsync_ValidId_CallsRepository()
    {
        // Arrange
        var wishlistItemId = Guid.NewGuid();
        var wishlist = new Wishlist { Id = wishlistItemId };

        _wishlistRepositoryMock.Setup(repo
            => repo.GetWishlistByIdAsync(wishlistItemId)).ReturnsAsync(wishlist);

        // Act
        await _wishlistService.DeleteWishlistItemAsync(wishlistItemId);

        // Assert
        _wishlistRepositoryMock.Verify(repo => repo.DeleteWishlist(wishlist), Times.Once);
        _repositoryManagerMock.Verify(rm => rm.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteWishlistItemByIdAsync_InvalidId_ThrowsNotFoundException()
    {
        // Arrange
        var wishlistId = Guid.NewGuid();
        _wishlistRepositoryMock.Setup(repo
            => repo.GetWishlistByIdAsync(wishlistId)).ReturnsAsync(null as Wishlist);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _wishlistService.DeleteWishlistItemAsync(wishlistId));
    }
}