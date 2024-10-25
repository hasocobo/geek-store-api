using StoreApi.Entities;

namespace StoreApi.Features.Carts
{
    public class CartService : ICartService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILogger<CartService> _logger;

        public CartService(IRepositoryManager repositoryManager, ILogger<CartService> logger)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
        }

        public async Task<IEnumerable<Cart>> GetCartsAsync()
        {
            return await _repositoryManager.CartRepository.GetCartsAsync();
        }

        public async Task<Cart> GetCartByIdAsync(Guid cartId)
        {
            return await _repositoryManager.CartRepository.GetCartByIdAsync(cartId);
        }
    }
}