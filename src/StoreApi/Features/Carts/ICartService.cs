using StoreApi.Common.DataTransferObjects.Carts;
using StoreApi.Entities;

namespace StoreApi.Features.Carts
{
    public interface ICartService
    {
        Task<IEnumerable<CartReadDto>> GetCartsAsync();
        Task<CartReadDto> GetCartByIdAsync(Guid cartId);
        Task<IEnumerable<CartReadDto>> GetCartsByCustomerIdAsync(Guid customerId);
        
        Task<CartReadDto> CreateCartForCustomerAsync(Guid customerId, CartCreateDto cartCreateDto);
        
        Task UpdateCartAsync(Guid customerId, Guid id, CartUpdateDto cartUpdateDto);
        Task DeleteCartAsync(Guid cartId);
        Task DeleteCartsForCustomerAsync(Guid customerId);
    }
}
