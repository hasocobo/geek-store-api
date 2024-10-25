using Microsoft.EntityFrameworkCore;
using StoreApi.Entities;
using StoreApi.Infrastructure;

namespace StoreApi.Features.Wishlists
{
    public class WishlistRepository : RepositoryBase<Wishlist>, IWishlistRepository
    {
        public WishlistRepository(StoreContext storeContext) : base(storeContext)
        {
        }

        public async Task<IEnumerable<Wishlist>> GetWishlistsAsync()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<Wishlist> GetWishlistByIdAsync(Guid id)
        {
            return await FindByCondition(wishlist =>
                           wishlist.Id.Equals(id))
                       .SingleOrDefaultAsync() ??
                   throw new InvalidOperationException();
        }
    }
}