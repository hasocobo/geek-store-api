using StoreApi.Common.DataTransferObjects.Orders;
using StoreApi.Entities;

namespace StoreApi.Features.Orders
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderReadDto>> GetOrdersAsync();
        Task<IEnumerable<OrderReadDto>> GetOrdersByCustomerIdAsync(Guid customerId);
        Task<OrderReadDto> GetOrderByIdAsync(Guid id);
        
        Task<OrderReadDto> CreateOrderForCustomerAsync(Guid customerId, OrderCreateDto orderCreateDto);
    }
}
