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
            return await FindAll().ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await FindByCondition(order => order.Id.Equals(orderId)).SingleOrDefaultAsync()
                   ?? throw new InvalidOperationException();
        }

        public void CreateOrder(Order order)
        {
            Create(order);
        }
    }
}