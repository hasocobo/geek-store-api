using StoreApi.Entities;

namespace StoreApi.Features.Orders
{
    public interface IOrderRepository
    {
        
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(Guid customerId);
        
        Task<Order> GetOrderByIdAsync(Guid orderId);
        
        void CreateOrder(Order order);
        void DeleteOrder(Order order);
    }
}
