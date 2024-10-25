using StoreApi.Entities;

namespace StoreApi.Features.Customers
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetCustomersAsync();
        Task<Customer> GetCustomerByIdAsync(Guid customerId);
    }
}
