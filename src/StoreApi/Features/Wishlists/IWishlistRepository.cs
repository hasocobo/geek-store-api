using StoreApi.Entities;

namespace StoreApi.Features.Wishlists
{
    public interface IWishlistRepository
    {
        Task<IEnumerable<Wishlist>> GetWishlistsAsync();
        Task<Wishlist> GetWishlistByIdAsync(Guid id);
    }
}
