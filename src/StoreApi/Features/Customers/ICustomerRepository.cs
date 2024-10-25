using StoreApi.Entities;

namespace StoreApi.Features.Customers
{
    public interface ICustomerRepository
    {
        
        Task<IEnumerable<Customer>> GetCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(Guid customerId);
    }
}
