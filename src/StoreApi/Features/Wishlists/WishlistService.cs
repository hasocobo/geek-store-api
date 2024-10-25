using StoreApi.Entities;

namespace StoreApi.Features.Wishlists
{
    public class WishlistService : IWishlistService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILogger<WishlistService> _logger;

        public WishlistService(IRepositoryManager repositoryManager, ILogger<WishlistService> logger)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
        }

        public async Task<IEnumerable<Wishlist>> GetWishlistsAsync()
        {
            return await _repositoryManager.WishlistRepository.GetWishlistsAsync();
        }
        public async Task<Wishlist> GetWishlistByIdAsync(Guid id)
        {
            return await _repositoryManager.WishlistRepository.GetWishlistByIdAsync(id);
        }
    }
}
