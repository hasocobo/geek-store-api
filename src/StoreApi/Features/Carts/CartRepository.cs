﻿using Microsoft.EntityFrameworkCore;
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
            return await FindAll().Include(ci => ci.Product).ToListAsync();
        }

        public async Task<Cart> GetCartByIdAsync(Guid cartId)
        {
            return await FindByCondition(cart => cart.Id.Equals(cartId))
                .Include(ci => ci.Product)
                .SingleOrDefaultAsync() ?? throw new InvalidOperationException();
        }

        public async Task<IEnumerable<Cart>> GetCartsByCustomerIdAsync(Guid customerId)
        {
            return await FindByCondition(cart => cart.CustomerId.Equals(customerId))
                .Include(ci => ci.Product)
                .ToListAsync();
        }

        public void AddToCart(Cart cart)
        {
            Create(cart);
        }

        public void DeleteCart(Cart cart)
        {
            Delete(cart);
        }
    }
}