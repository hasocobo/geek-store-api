using StoreApi.Entities;

namespace StoreApi.Features.Wishlists
{
    public interface IWishlistService
    {
        Task<IEnumerable<Wishlist>> GetWishlistsAsync();
        Task<Wishlist> GetWishlistByIdAsync(Guid id);
        
        Task CreateWishlistAsync(Wishlist wishlist);
    }
}
