using StoreApi.Entities;
using StoreApi.Infrastructure;

namespace StoreApi.Features.Wishlists
{
    public class WishlistRepository : RepositoryBase<Wishlist>, IWishlistRepository
    {
        public WishlistRepository(StoreContext storeContext) : base(storeContext)
        {
        }
    }
}
