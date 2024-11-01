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
            return await FindAll()
                .Include(wi => wi.Product)
                .ToListAsync();
        }

        public async Task<Wishlist> GetWishlistByIdAsync(Guid id)
        {
            return await FindByCondition(wishlist =>
                           wishlist.Id.Equals(id))
                       .Include(wi => wi.Product)
                       .SingleOrDefaultAsync() ??
                   throw new InvalidOperationException();
        }

        public async Task<IEnumerable<Wishlist>> GetWishlistByCustomerIdAsync(Guid customerId)
        {
            return await FindByCondition(wi => wi.CustomerId.Equals(customerId))
                .Include(wi => wi.Product)
                .ToListAsync();
        }

        public void CreateWishlist(Wishlist wishlist)
        {
            Create(wishlist);
        }

        public void DeleteWishlist(Wishlist wishlist)
        {
            Delete(wishlist);
        }
    }
}