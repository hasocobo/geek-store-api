using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using StoreApi.Common.DataTransferObjects.Carts;
using StoreApi.Entities;
using StoreApi.Entities.Exceptions;
using StoreApi.Features;
using StoreApi.Features.Carts;
using StoreApi.Features.Customers;
using StoreApi.Features.Products;
using Xunit;

namespace StoreApiTests.CartTests;

public class CartUnitTests
{
    private readonly Mock<IRepositoryManager> _repositoryManagerMock;
    private readonly Mock<ICartRepository> _cartRepositoryMock;
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly ICartService _cartService;

    public CartUnitTests()
    {
        _cartRepositoryMock = new Mock<ICartRepository>();
        _repositoryManagerMock = new Mock<IRepositoryManager>();
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _productRepositoryMock = new Mock<IProductRepository>();

        _repositoryManagerMock.Setup(rm => rm.CartRepository).Returns(_cartRepositoryMock.Object);
        _repositoryManagerMock.Setup(rm => rm.CustomerRepository).Returns(_customerRepositoryMock.Object);
        _repositoryManagerMock.Setup(rm => rm.ProductRepository).Returns(_productRepositoryMock.Object);
        _repositoryManagerMock.Setup(rm => rm.SaveAsync()).Returns(Task.CompletedTask);

        _cartService = new CartService(_repositoryManagerMock.Object, NullLogger<CartService>.Instance);
    }

    [Fact]
    public async Task GetCartsAsync_ReturnsAllCarts()
    {
        // Arrange
        var carts = new List<Cart>
        {
            new()
            {
                Id = Guid.NewGuid(), ProductId = Guid.NewGuid(), Quantity = 1,
                Product = new Product { Name = "Product 1", Price = 10.0M }
            },
            new()
            {
                Id = Guid.NewGuid(), ProductId = Guid.NewGuid(), Quantity = 2,
                Product = new Product { Name = "Product 2", Price = 20.0M }
            }
        };

        _cartRepositoryMock.Setup(repo => repo.GetCartsAsync()).ReturnsAsync(carts);

        // Act
        var result = await _cartService.GetCartsAsync();

        // Assert
        Assert.Equal(carts.Count, result.Count());
    }

    [Fact]
    public async Task GetCartItemByIdAsync_ValidId_ReturnsCart()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var cart = new Cart
        {
            Id = cartId,
            ProductId = Guid.NewGuid(),
            Quantity = 1,
            Product = new Product { Name = "Product", Price = 10.0M }
        };

        _cartRepositoryMock.Setup(repo => repo.GetCartItemByIdAsync(cartId)).ReturnsAsync(cart);

        // Act
        var result = await _cartService.GetCartItemByIdAsync(cartId);

        // Assert
        Assert.Equal(cartId, result.Id);
    }

    [Fact]
    public async Task GetCartItemByIdAsync_InvalidId_ThrowsNotFoundException()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        _cartRepositoryMock.Setup(repo => repo.GetCartItemByIdAsync(cartId)).ReturnsAsync(null as Cart);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _cartService.GetCartItemByIdAsync(cartId));
    }

    [Fact]
    public async Task GetCartByCustomerIdAsync_ValidId_ReturnsCarts()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var carts = new List<Cart>
        {
            new()
            {
                Id = Guid.NewGuid(), ProductId = Guid.NewGuid(), Quantity = 1,
                Product = new Product { Name = "Test Product 1", Price = 10.0M }
            },
            new()
            {
                Id = Guid.NewGuid(), ProductId = Guid.NewGuid(), Quantity = 2,
                Product = new Product { Name = "Test Product 2", Price = 20.0M }
            }
        };
        _customerRepositoryMock.Setup(r => r.CheckIfCustomerExists(customerId)).ReturnsAsync(true);
        _cartRepositoryMock.Setup(r => r.GetCartByCustomerIdAsync(customerId)).ReturnsAsync(carts);
        
        // Act
        var result = await _cartService.GetCartByCustomerIdAsync(customerId);
        
        // Assert
        Assert.Equal(carts.Count, result.Count());
    }

    [Fact]
    public async Task GetCartByCustomerIdAsync_InvalidId_ThrowsNotFoundException()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        _customerRepositoryMock.Setup(r => r.CheckIfCustomerExists(customerId)).ReturnsAsync(false);
        
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _cartService.GetCartByCustomerIdAsync(customerId));
    }

    [Fact]
    public async Task CreateCartItemForCustomerAsync_ValidInput_ReturnsCreatedCart()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var product = new Product { Id = productId, Name = "Test Product", Price = 10.0M };

        var cartCreateDto = new CartCreateDto(ProductId: productId, Quantity: 1);
        var createdCart = new CartReadDto(Id: Guid.NewGuid(), ProductId: productId, CustomerId: customerId, Quantity: 1,
            ProductName: product.Name, UnitPrice: product.Price);

        _customerRepositoryMock.Setup(repo
            => repo.CheckIfCustomerExists(customerId)).ReturnsAsync(true);
        _productRepositoryMock.Setup(repo
            => repo.CheckIfProductExists(productId)).ReturnsAsync(true);
        _productRepositoryMock.Setup(repo
            => repo.GetProductByIdAsync(productId)).ReturnsAsync(product);

        _cartRepositoryMock.Setup(repo => repo.AddToCart(It.IsAny<Cart>()));

        // Act
        var result = await _cartService.CreateCartItemForCustomerAsync(customerId, cartCreateDto);

        // Assert
        Assert.Equal(createdCart.CustomerId, result.CustomerId);
        Assert.Equal(cartCreateDto.ProductId, result.ProductId);
        _cartRepositoryMock.Verify(repo => repo.AddToCart(It.IsAny<Cart>()), Times.Once);
        _repositoryManagerMock.Verify(repo => repo.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateCartItemForCustomerAsync_InvalidCustomerId_ReturnsCreatedCart()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var cartCreateDto = new CartCreateDto(ProductId: productId, Quantity: 1);
        _customerRepositoryMock.Setup(r 
            => r.CheckIfCustomerExists(customerId)).ReturnsAsync(false);
        _productRepositoryMock.Setup(r 
            => r.CheckIfProductExists(productId)).ReturnsAsync(true);
        
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _cartService.CreateCartItemForCustomerAsync(customerId, cartCreateDto: cartCreateDto));
    }
    
    [Fact]
    public async Task CreateCartItemForCustomerAsync_InvalidProductId_ReturnsCreatedCart()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var cartCreateDto = new CartCreateDto(ProductId: productId, Quantity: 1);
        _customerRepositoryMock.Setup(r 
            => r.CheckIfCustomerExists(customerId)).ReturnsAsync(true);
        _productRepositoryMock.Setup(r 
            => r.CheckIfProductExists(productId)).ReturnsAsync(false);
        
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _cartService.CreateCartItemForCustomerAsync(customerId, cartCreateDto: cartCreateDto));
    }

    [Fact]
    public async Task UpdateCartItemAsync_ValidInput_UpdatesCart()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var cartUpdateDto = new CartUpdateDto(Quantity: 3);

        var cart = new Cart { Id = cartId, CustomerId = customerId, Quantity = 1 };

        _customerRepositoryMock.Setup(repo => repo.CheckIfCustomerExists(customerId)).ReturnsAsync(true);
        _cartRepositoryMock.Setup(repo => repo.GetCartItemByIdAsync(cartId)).ReturnsAsync(cart);

        // Act
        await _cartService.UpdateCartItemAsync(customerId, cartId, cartUpdateDto);

        // Assert
        Assert.Equal(cartUpdateDto.Quantity, cart.Quantity);
        _repositoryManagerMock.Verify(repo => repo.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateCartItemAsync_InvalidCartId_ThrowsNotFoundException()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var cartUpdateDto = new CartUpdateDto(Quantity: 3);

        _cartRepositoryMock.Setup(repo => repo.GetCartItemByIdAsync(cartId)).ReturnsAsync(null as Cart);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _cartService.UpdateCartItemAsync(customerId, cartId, cartUpdateDto));
    }

    [Fact]
    public async Task DeleteCartItemByIdAsync_ValidId_CallsRepository()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var cart = new Cart { Id = cartId };

        _cartRepositoryMock.Setup(repo => repo.GetCartItemByIdAsync(cartId)).ReturnsAsync(cart);

        // Act
        await _cartService.DeleteCartItemByIdAsync(cartId);

        // Assert
        _cartRepositoryMock.Verify(repo => repo.DeleteCartItem(cart), Times.Once);
        _repositoryManagerMock.Verify(repo => repo.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteCartItemByIdAsync_InvalidId_ThrowsNotFoundException()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        _cartRepositoryMock.Setup(repo => repo.GetCartItemByIdAsync(cartId)).ReturnsAsync(null as Cart);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _cartService.DeleteCartItemByIdAsync(cartId));
    }

    [Fact]
    public async Task DeleteCartByCustomerIdAsync_ValidId_CallsRepository()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var cart = new List<Cart>
        {
            new()
            {
                Id = Guid.NewGuid(), ProductId = Guid.NewGuid(), Quantity = 1,
                Product = new Product { Name = "Test Product 1", Price = 10.0M }
            },
            new()
            {
                Id = Guid.NewGuid(), ProductId = Guid.NewGuid(), Quantity = 2,
                Product = new Product { Name = "Test Product 2", Price = 20.0M }
            },
            new()
            {
                Id = Guid.NewGuid(), ProductId = Guid.NewGuid(), Quantity = 3,
                Product = new Product { Name = "Test Product 3", Price = 30.0M }
            }
        };
        
        _customerRepositoryMock.Setup(r => r.CheckIfCustomerExists(customerId)).ReturnsAsync(true);
        _cartRepositoryMock.Setup(r => r.GetCartByCustomerIdAsync(customerId)).ReturnsAsync(cart);
        
        // Act
        await _cartService.DeleteCartForCustomerAsync(customerId);
        
        // Assert
        _cartRepositoryMock.Verify(r => r.DeleteCartItem(It.IsAny<Cart>()), Times.Exactly(cart.Count));
    }

    [Fact]
    public async Task DeleteCartByCustomerIdAsync_InvalidId_ThrowsNotFoundException()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        _customerRepositoryMock.Setup(r => r.CheckIfCustomerExists(customerId)).ReturnsAsync(false);
        
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _cartService.DeleteCartForCustomerAsync(customerId));
    }
}