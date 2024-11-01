using Microsoft.EntityFrameworkCore;
using StoreApi.Entities;
using StoreApi.Infrastructure;

namespace StoreApi.Features.Orders
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(StoreContext storeContext) : base(storeContext)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await FindAll()
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(Guid customerId)
        {
            return await FindByCondition(o => o.CustomerId.Equals(customerId))
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await FindByCondition(order => order.Id.Equals(orderId))
                       .Include(o => o.OrderItems)
                       .ThenInclude(oi => oi.Product)
                       .SingleOrDefaultAsync()
                   ?? throw new InvalidOperationException();
        }

        public void CreateOrder(Order order)
        {
            Create(order);
        }

        public void DeleteOrder(Order order)
        {
            Delete(order);
        }
    }
}