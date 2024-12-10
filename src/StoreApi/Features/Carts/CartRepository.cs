using Microsoft.EntityFrameworkCore;
using StoreApi.Entities;
using StoreApi.Entities.Exceptions;
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
            return await FindAll().Include(ci => ci.Product).ToListAsync();
        }

        public async Task<Cart?> GetCartItemByIdAsync(Guid cartId)
        {
            return await FindByCondition(cart => cart.Id.Equals(cartId))
                .Include(ci => ci.Product)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Cart>> GetCartByCustomerIdAsync(Guid customerId)
        {
            return await FindByCondition(cart => cart.CustomerId.Equals(customerId))
                .Include(ci => ci.Product)
                .ToListAsync();
        }

        public void AddToCart(Cart cart)
        {
            Create(cart);
        }

        public void UpdateCartItem(Cart cart)
        {
            Update(cart);
        }

        public void DeleteCartItem(Cart cart)
        {
            Delete(cart);
        }
    }
}