using StoreApi.Entities;
using StoreApi.Infrastructure;

namespace StoreApi.Features.Customers
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(StoreContext storeContext) : base(storeContext)
        {
        }
    }
}
