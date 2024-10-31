using StoreApi.Common.DataTransferObjects.Wishlists;
using StoreApi.Entities;

namespace StoreApi.Features.Wishlists
{
    public interface IWishlistService
    {
        Task<IEnumerable<WishlistReadDto>> GetWishlistsAsync();
        Task<WishlistReadDto> GetWishlistItemByIdAsync(Guid id);
        Task<IEnumerable<WishlistReadDto>> GetWishlistByCustomerIdAsync(Guid customerId);
        
        Task<WishlistReadDto> CreateWishlistItemForCustomerAsync(Guid customerId, WishlistCreateDto wishlist);
        
        Task DeleteWishlistItemAsync(Guid id);
    }
}
