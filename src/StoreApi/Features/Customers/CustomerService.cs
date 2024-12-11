using StoreApi.Entities;
using StoreApi.Entities.Exceptions;

namespace StoreApi.Features.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(IRepositoryManager repositoryManager, ILogger<CustomerService> logger)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            return await _repositoryManager.CustomerRepository.GetCustomersAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(Guid id)
        {
            return await _repositoryManager.CustomerRepository.GetCustomerByIdAsync(id) ??
                   throw new NotFoundException("Customer", id);
        }
    }
}