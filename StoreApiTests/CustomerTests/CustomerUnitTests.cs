using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using StoreApi.Features;
using StoreApi.Features.Customers;

namespace StoreApiTests.CustomerTests;

public class CustomerUnitTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly Mock<IRepositoryManager> _repositoryManagerMock;
    private readonly ICustomerService _customerService;

    public CustomerUnitTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _repositoryManagerMock = new Mock<IRepositoryManager>();

        _repositoryManagerMock.Setup(rm
            => rm.CustomerRepository).Returns(_customerRepositoryMock.Object);

        _repositoryManagerMock.Setup(rm
            => rm.SaveAsync()).Returns(Task.CompletedTask);

        _customerService = new CustomerService(_repositoryManagerMock.Object, NullLogger<CustomerService>.Instance);
    }
}