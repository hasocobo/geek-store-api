using StoreApi.Entities;

namespace StoreApi.Features.Carts
{
    public interface ICartRepository
    {
        Task<IEnumerable<Cart>> GetCartsAsync();
        Task<Cart?> GetCartItemByIdAsync(Guid cartId);
        Task<IEnumerable<Cart>> GetCartByCustomerIdAsync(Guid customerId);
        
        void AddToCart(Cart cart);
        void UpdateCartItem(Cart cart);
        void DeleteCartItem(Cart cart);
    }
}
