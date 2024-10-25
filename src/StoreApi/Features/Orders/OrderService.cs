using StoreApi.Entities;

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

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _repositoryManager.OrderRepository.GetOrdersAsync();
        }

        public Task<Order> GetOrderByIdAsync(Guid id)
        {
            return _repositoryManager.OrderRepository.GetOrderByIdAsync(id);
        }
    }
}