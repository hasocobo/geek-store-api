using StoreApi.Common.DataTransferObjects.Carts;
using StoreApi.Entities;

namespace StoreApi.Features.Carts
{
    public interface ICartService
    {
        Task<IEnumerable<CartReadDto>> GetCartsAsync();
        Task<CartReadDto> GetCartItemByIdAsync(Guid cartId);
        Task<IEnumerable<CartReadDto>> GetCartByCustomerIdAsync(Guid customerId);
        
        Task<CartReadDto> CreateCartItemForCustomerAsync(Guid customerId, CartCreateDto cartCreateDto);
        
        Task UpdateCartItemAsync(Guid customerId, Guid id, CartUpdateDto cartUpdateDto);
        Task DeleteCartItemByIdAsync(Guid cartId);
        Task DeleteCartForCustomerAsync(Guid customerId);
    }
}
