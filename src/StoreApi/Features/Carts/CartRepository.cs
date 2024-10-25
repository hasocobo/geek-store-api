using Microsoft.EntityFrameworkCore;
using StoreApi.Entities;
using StoreApi.Infrastructure;

namespace StoreApi.Features.Carts
{
    public class CartRepository : RepositoryBase<Cart>, ICartRepository
    {
        public CartRepository(StoreContext storeContext) : base(storeContext)
        {
        }


        public async Task<IEnumerable<Cart>> GetCartsAsync()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<Cart> GetCartByIdAsync(Guid cartId)
        {
            return await FindByCondition(cart => cart.Id.Equals(cartId))
                .SingleOrDefaultAsync() ?? throw new InvalidOperationException();
        }
    }
}