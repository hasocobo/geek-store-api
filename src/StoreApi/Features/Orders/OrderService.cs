namespace StoreApi.Features.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IRepositoryManager repositoryManager, ILogger<OrderService> logger)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
        }
        
    }
}
