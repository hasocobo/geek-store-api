using StoreApi.Common.DataTransferObjects.Carts;
using StoreApi.Entities;

namespace StoreApi.Features.Carts
{
    public interface ICartService
    {
        Task<IEnumerable<Cart>> GetCartsAsync();
        Task<Cart> GetCartByIdAsync(Guid cartId);
        
        Task<CartReadDto> CreateCartForCustomerAsync(Guid customerId, CartCreateDto cartCreateDto);
        
    }
}
