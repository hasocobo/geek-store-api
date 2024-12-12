using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using StoreApi.Common.DataTransferObjects.Orders;
using StoreApi.Entities;
using StoreApi.Entities.Exceptions;
using StoreApi.Features;
using StoreApi.Features.Carts;
using StoreApi.Features.Customers;
using StoreApi.Features.Orders;
using StoreApi.Features.Products;

namespace StoreApiTests.OrderTests;

public class OrderUnitTests
{
    private readonly Mock<ICartRepository> _cartRepositoryMock;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly Mock<IRepositoryManager> _repositoryManagerMock;
    private readonly Mock<IDbContextTransaction> _transactionMock;

    private readonly IOrderService _orderService;

    public OrderUnitTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _repositoryManagerMock = new Mock<IRepositoryManager>();
        _cartRepositoryMock = new Mock<ICartRepository>();
        _transactionMock = new Mock<IDbContextTransaction>();

        _repositoryManagerMock.Setup(rm
            => rm.OrderRepository).Returns(_orderRepositoryMock.Object);

        _repositoryManagerMock.Setup(rm
            => rm.ProductRepository).Returns(_productRepositoryMock.Object);

        _repositoryManagerMock.Setup(rm
            => rm.CustomerRepository).Returns(_customerRepositoryMock.Object);

        _repositoryManagerMock.Setup(rm
            => rm.BeginTransactionAsync()).ReturnsAsync(_transactionMock.Object);

        _repositoryManagerMock.Setup(rm
            => rm.CartRepository).Returns(_cartRepositoryMock.Object);

        _repositoryManagerMock.Setup(rm
            => rm.SaveAsync()).Returns(Task.CompletedTask);

        _orderService = new OrderService(_repositoryManagerMock.Object, NullLogger<OrderService>.Instance);
    }

    [Fact]
    public async Task GetOrdersAsync_ReturnsAllOrders()
    {
        // Arrange
        var orders = new List<Order>
        {
            new Order { Id = Guid.NewGuid(), CustomerId = Guid.NewGuid(), Date = DateTime.Now },
            new Order { Id = Guid.NewGuid(), CustomerId = Guid.NewGuid(), Date = DateTime.Now }
        };

        _orderRepositoryMock.Setup(or => or.GetOrdersAsync()).ReturnsAsync(orders);

        // Act
        var result = (await _orderService.GetOrdersAsync()).ToList();

        // Assert
        Assert.Equal(orders.Count, result.Count);
        Assert.Equal(orders.First().Id, result.First().Id);
        Assert.Equal(orders.First().CustomerId, result.First().CustomerId);
    }

    [Fact]
    public async Task GetOrderByIdAsync_ValidId_ReturnsOrder()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = new Order { Id = orderId, CustomerId = Guid.NewGuid(), Date = DateTime.Now };

        _orderRepositoryMock.Setup(or => or.GetOrderByIdAsync(orderId)).ReturnsAsync(order);

        // Act
        var result = await _orderService.GetOrderByIdAsync(orderId);

        // Assert
        Assert.Equal(orderId, result.Id);
        Assert.Equal(order.CustomerId, result.CustomerId);
    }

    [Fact]
    public async Task GetOrderByIdAsync_InvalidId_ThrowsNotFoundException()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        _orderRepositoryMock.Setup(or => or.GetOrderByIdAsync(orderId)).ReturnsAsync(null as Order);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _orderService.GetOrderByIdAsync(orderId));
    }

    [Fact]
    public async Task GetOrdersByCustomerIdAsync_ValidId_ReturnsOrders()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var orders = new List<Order>
        {
            new Order { Id = Guid.NewGuid(), CustomerId = customerId, Date = DateTime.Now },
            new Order { Id = Guid.NewGuid(), CustomerId = customerId, Date = DateTime.Now }
        };

        _customerRepositoryMock.Setup(cr => cr.CheckIfCustomerExists(customerId)).ReturnsAsync(true);
        _orderRepositoryMock.Setup(or => or.GetOrdersByCustomerIdAsync(customerId)).ReturnsAsync(orders);

        // Act
        var result = (await _orderService.GetOrdersByCustomerIdAsync(customerId)).ToList();

        // Assert
        Assert.Equal(orders.Count, result.Count);
        Assert.Equal(orders.First().CustomerId, result.First().CustomerId);
    }

    [Fact]
    public async Task GetOrdersByCustomerIdAsync_InvalidId_ThrowsNotFoundException()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        _customerRepositoryMock.Setup(cr => cr.CheckIfCustomerExists(customerId)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _orderService.GetOrdersByCustomerIdAsync(customerId));
    }

    [Fact]
    public async Task CreateOrderForCustomerAsync_ValidInput_ReturnsCreatedOrder()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var cartItems = new List<Cart>
        {
            new Cart
            {
                Id = Guid.NewGuid(), CustomerId = customerId, ProductId = Guid.NewGuid(), Quantity = 1,
                Product = new Product { Name = "Test product", Price = 100 }
            },
            new Cart
            {
                Id = Guid.NewGuid(), CustomerId = customerId, ProductId = Guid.NewGuid(), Quantity = 2,
                Product = new Product { Name = "Test product 2", Price = 200 }
            }
        };
        _customerRepositoryMock.Setup(cr
            => cr.CheckIfCustomerExists(customerId)).ReturnsAsync(true);

        _cartRepositoryMock.Setup(cr
            => cr.GetCartByCustomerIdAsync(customerId)).ReturnsAsync(cartItems);

        _cartRepositoryMock.Setup(cr
            => cr.DeleteCartItem(It.IsAny<Cart>()));

        _transactionMock.Setup(t
            => t.CommitAsync(CancellationToken.None)).Returns(Task.CompletedTask);

        // Act
        var result = await _orderService.CreateOrderForCustomerAsync(customerId, new OrderCreateDto());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(customerId, result.CustomerId);
        Assert.Equal(cartItems.Count, result.OrderItems.Count);
        Assert.Equal(500, result.TotalPrice);

        _orderRepositoryMock.Verify(or
            => or.CreateOrder(It.IsAny<Order>()), Times.Once);
        _repositoryManagerMock.Verify(rm
            => rm.SaveAsync(), Times.Once);
        _transactionMock.Verify(tm
            => tm.CommitAsync(CancellationToken.None), Times.Once);
        _cartRepositoryMock.Verify(cr
            => cr.DeleteCartItem(It.IsAny<Cart>()), Times.Exactly(cartItems.Count));
    }

    [Fact]
    public async Task
        CreateOrderForCustomerAsync_ValidInput_WhenFailsToCreateOrder_RollbacksAndThrowsOperationCanceledException()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var cartItems = new List<Cart>
        {
            new Cart
            {
                Id = Guid.NewGuid(), CustomerId = customerId, ProductId = Guid.NewGuid(), Quantity = 1,
                Product = new Product { Name = "Test product", Price = 100 }
            },
            new Cart
            {
                Id = Guid.NewGuid(), CustomerId = customerId, ProductId = Guid.NewGuid(), Quantity = 2,
                Product = new Product { Name = "Test product 2", Price = 200 }
            }
        };
        _customerRepositoryMock.Setup(cr
            => cr.CheckIfCustomerExists(customerId)).ReturnsAsync(true);

        _cartRepositoryMock.Setup(cr
            => cr.GetCartByCustomerIdAsync(customerId)).ReturnsAsync(cartItems);

        _cartRepositoryMock.Setup(cr
            => cr.DeleteCartItem(It.IsAny<Cart>()));

        _transactionMock.Setup(t
            => t.CommitAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new OperationCanceledException());

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            _orderService.CreateOrderForCustomerAsync(customerId, new OrderCreateDto()));

        _transactionMock.Verify(tm => tm.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateOrderForCustomerAsync_InvalidCustomerId_ThrowsNotFoundException()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        _customerRepositoryMock.Setup(cr => cr.CheckIfCustomerExists(customerId)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _orderService.CreateOrderForCustomerAsync(customerId, new OrderCreateDto()));
    }

    [Fact]
    public async Task CreateOrderForCustomerAsync_EmptyCart_ThrowsNotFoundException()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        _customerRepositoryMock.Setup(cr => cr.CheckIfCustomerExists(customerId)).ReturnsAsync(true);
        _repositoryManagerMock.Setup(rm => rm.CartRepository.GetCartByCustomerIdAsync(customerId))
            .ReturnsAsync(new List<Cart>());

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _orderService.CreateOrderForCustomerAsync(customerId, new OrderCreateDto()));
    }

    [Fact]
    public async Task DeleteOrderAsync_ValidId_CallsRepository()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderToDelete = new Order { CustomerId = Guid.NewGuid(), Id = orderId };
        _orderRepositoryMock.Setup(or
            => or.GetOrderByIdAsync(orderId)).ReturnsAsync(orderToDelete);
        _orderRepositoryMock.Setup(or => or.DeleteOrder(orderToDelete));

        // Act
        await _orderService.DeleteOrderAsync(orderId);

        // Assert
        _orderRepositoryMock.Verify(or => or.DeleteOrder(orderToDelete), Times.Once);
        _repositoryManagerMock.Verify(rm => rm.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteOrderAsync_InvalidId_ThrowsNotFoundException()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        _orderRepositoryMock.Setup(or
            => or.GetOrderByIdAsync(orderId)).ReturnsAsync(null as Order);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _orderService.DeleteOrderAsync(orderId));
    }
}