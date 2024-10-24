using StoreApi.Entities;
using StoreApi.Infrastructure;

namespace StoreApi.Features.Orders
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(StoreContext storeContext) : base(storeContext)
        {
        }
    }
}
