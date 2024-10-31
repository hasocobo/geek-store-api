using StoreApi.Common.DataTransferObjects.Wishlists;
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

        public async Task<IEnumerable<WishlistReadDto>> GetWishlistsAsync()
        {
            _logger.LogInformation("Getting all wishlist items");
            var wishlistItems = await _repositoryManager.WishlistRepository.GetWishlistsAsync();

            _logger.LogInformation("Converting all wishlist items to read-only wishlist data transfer objects.");
            var wishlistItemsToReturn = wishlistItems.Select
            (
                wi =>
                    new WishlistReadDto
                    (
                        Id: wi.Id,
                        ProductId: wi.ProductId,
                        ProductName: wi.Product.Name,
                        ProductDescription: wi.Product.Description
                    )
            );
            return wishlistItemsToReturn;
        }

        public async Task<WishlistReadDto> GetWishlistItemByIdAsync(Guid id)
        {
            _logger.LogInformation($"Getting wishlist with id: {id}");
            var wishlistItem = await _repositoryManager.WishlistRepository.GetWishlistByIdAsync(id);

            _logger.LogInformation($"Converting wishlist item to read-only wishlist object.");
            var wishlistItemToReturn = new WishlistReadDto
            (
                Id: wishlistItem.Id,
                ProductId: wishlistItem.ProductId,
                ProductName: wishlistItem.Product.Name,
                ProductDescription: wishlistItem.Product.Description
            );

            return wishlistItemToReturn;
        }

        public async Task<IEnumerable<WishlistReadDto>> GetWishlistByCustomerIdAsync(Guid customerId)
        {
            _logger.LogInformation($"Getting wishlist items by customer: {customerId}");
            var wishlist = await
                _repositoryManager.WishlistRepository.GetWishlistByCustomerIdAsync(customerId);

            _logger.LogInformation($"Returning wishlist by converting it to read-only.");
            var wishlistToReturn = wishlist.Select(wi =>
                new WishlistReadDto
                (
                    Id: wi.Id,
                    ProductId: wi.ProductId,
                    ProductName: wi.Product.Name,
                    ProductDescription: wi.Product.Description
                )
            );
            
            return wishlistToReturn;
        }

        public async Task<WishlistReadDto> CreateWishlistItemForCustomerAsync(Guid customerId,
            WishlistCreateDto wishlistCreateDto)
        {
            _logger.LogInformation($"Creating wishlist for customer: {customerId}");
            var wishlistItem = new Wishlist
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                ProductId = wishlistCreateDto.ProductId
            };
            _repositoryManager.WishlistRepository.CreateWishlist(customerId, wishlistItem);
            await _repositoryManager.SaveAsync();

            _logger.LogInformation($"Wishlist for customer: {customerId} successfully created");

            _logger.LogInformation($"Fetching product with id: {wishlistCreateDto.ProductId}");
            var product = await _repositoryManager.ProductRepository.GetProductByIdAsync(wishlistCreateDto.ProductId);

            _logger.LogInformation($"Converting wishlist item to read-only wishlist item.");
            var wishlistItemToReturn = new WishlistReadDto
            (
                Id: wishlistItem.Id,
                ProductId: wishlistCreateDto.ProductId,
                ProductName: product.Name,
                ProductDescription: product.Description
            );

            return wishlistItemToReturn;
        }

        public async Task DeleteWishlistItemAsync(Guid id)
        {
            _logger.LogInformation($"Fetching wishlist with id: {id} to delete");
            var wishlistToDelete = await _repositoryManager.WishlistRepository.GetWishlistByIdAsync(id);

            _logger.LogInformation($"Deleting wishlist with id: {id}");
            _repositoryManager.WishlistRepository.DeleteWishlist(wishlistToDelete);

            await _repositoryManager.SaveAsync();
        }
    }
}