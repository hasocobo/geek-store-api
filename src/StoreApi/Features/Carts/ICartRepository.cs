using StoreApi.Entities;

namespace StoreApi.Features.Carts
{
    public interface ICartRepository
    {
        Task<IEnumerable<Cart>> GetCartsAsync();
        Task<Cart> GetCartByIdAsync(Guid cartId);
    }
}
