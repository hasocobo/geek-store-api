using StoreApi.Entities;

namespace StoreApi.Features.Orders
{
    public interface IOrderRepository
    {
        
        Task<IEnumerable<Order>> GetOrdersAsync();
        
        Task<Order> GetOrderByIdAsync(Guid orderId);
        
        void CreateOrder(Order order);
    }
}
