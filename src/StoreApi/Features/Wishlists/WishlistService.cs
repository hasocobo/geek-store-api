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
    }
}
