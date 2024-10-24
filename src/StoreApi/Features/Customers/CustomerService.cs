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
    }
}
