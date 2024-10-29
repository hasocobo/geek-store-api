using StoreApi.Entities;

namespace StoreApi.Features.Carts
{
    public interface ICartRepository
    {
        Task<IEnumerable<Cart>> GetCartsAsync();
        Task<Cart> GetCartByIdAsync(Guid cartId);
        Task<IEnumerable<Cart>> GetCartsByCustomerIdAsync(Guid customerId);
        void CreateCartForCustomer(Guid customerId, Cart cart);

        void DeleteCart(Cart cart);
    }
}
