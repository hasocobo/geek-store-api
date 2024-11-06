using Microsoft.EntityFrameworkCore;
using StoreApi.Entities;
using StoreApi.Entities.Exceptions;
using StoreApi.Infrastructure;

namespace StoreApi.Features.Customers
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(StoreContext storeContext) : base(storeContext)
        {
        }

        public async Task<bool> CheckIfCustomerExists(Guid customerId)
        {
            return await Exists(c => c.Id.Equals(customerId));
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            return await FindAll()
                .Include(customer => customer.User)
                .Include(customer => customer.Orders)
                .Include(customer => customer.Carts)
                .Include(customer => customer.Wishlists)
                .ToListAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(Guid customerId)
        {
            return await FindByCondition(customer =>
                    customer.Id.Equals(customerId))
                .Include(customer => customer.User)
                .SingleOrDefaultAsync() ?? throw new NotFoundException("Customer", customerId);
        }

        public void CreateCustomer(Customer customer)
        {
            Create(customer);
        }
    }
}