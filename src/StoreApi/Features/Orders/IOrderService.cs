using StoreApi.Entities;

namespace StoreApi.Features.Orders
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<Order> GetOrderByIdAsync(Guid id);
    }
}
