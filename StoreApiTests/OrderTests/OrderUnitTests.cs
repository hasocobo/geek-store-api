using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using StoreApi.Features;
using StoreApi.Features.Orders;

namespace StoreApiTests.OrderTests;

public class OrderUnitTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IRepositoryManager> _repositoryManagerMock;
    private readonly IOrderService _orderService;

    public OrderUnitTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _repositoryManagerMock = new Mock<IRepositoryManager>();

        _repositoryManagerMock.Setup(rm
            => rm.OrderRepository).Returns(_orderRepositoryMock.Object);

        _repositoryManagerMock.Setup(rm 
            => rm.SaveAsync()).Returns(Task.CompletedTask);

        _orderService = new OrderService(_repositoryManagerMock.Object, NullLogger<OrderService>.Instance);
    }
}