using StoreApi.Common.DataTransferObjects.Wishlists;
using StoreApi.Entities;
using StoreApi.Entities.Exceptions;

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
            var wishlistItems =
                await _repositoryManager.WishlistRepository.GetWishlistsAsync();

            _logger.LogInformation("Converting all wishlist items to read-only wishlist data transfer objects.");
            var wishlistItemsToReturn = wishlistItems.Select
            (
                wi =>
                    new WishlistReadDto
                    (
                        Id: wi.Id,
                        CustomerId: wi.CustomerId,
                        ProductId: wi.ProductId,
                        ProductName: wi.Product!.Name,
                        ProductDescription: wi.Product.Description
                    )
            );
            return wishlistItemsToReturn;
        }

        public async Task<WishlistReadDto> GetWishlistItemByIdAsync(Guid id)
        {
            _logger.LogInformation($"Getting wishlist with id: {id}");
            var wishlistItem =
                await _repositoryManager.WishlistRepository.GetWishlistByIdAsync(id);
            if (wishlistItem is null)
                throw new NotFoundException("Wishlist item", id);
            
            _logger.LogInformation($"Converting wishlist item to read-only wishlist object.");
            var wishlistItemToReturn = new WishlistReadDto
            (
                Id: wishlistItem.Id,
                CustomerId: wishlistItem.CustomerId,
                ProductId: wishlistItem.ProductId,
                ProductName: wishlistItem.Product!.Name,
                ProductDescription: wishlistItem.Product.Description
            );

            return wishlistItemToReturn;
        }

        public async Task<IEnumerable<WishlistReadDto>> GetWishlistByCustomerIdAsync(Guid customerId)
        {
            if (!await _repositoryManager.CustomerRepository.CheckIfCustomerExists(customerId))
                throw new NotFoundException("Customer", customerId);
            
            _logger.LogInformation($"Getting wishlist items by customer: {customerId}");
            var wishlist = await
                _repositoryManager.WishlistRepository.GetWishlistByCustomerIdAsync(customerId);

            _logger.LogInformation($"Returning wishlist for customer with ID: {customerId}");
            var wishlistToReturn = wishlist.Select(wi =>
                new WishlistReadDto
                (
                    Id: wi.Id,
                    CustomerId: wi.CustomerId,
                    ProductId: wi.ProductId,
                    ProductName: wi.Product!.Name,
                    ProductDescription: wi.Product.Description
                )
            );
            
            return wishlistToReturn;
        }

        public async Task<WishlistReadDto> CreateWishlistItemForCustomerAsync(Guid customerId,
            WishlistCreateDto wishlistCreateDto)
        {
            if (!await _repositoryManager.CustomerRepository.CheckIfCustomerExists(customerId))
                throw new NotFoundException("Customer", customerId);
            
            if (!await _repositoryManager.ProductRepository.CheckIfProductExists(wishlistCreateDto.ProductId))
                throw new NotFoundException("Product", wishlistCreateDto.ProductId);
            
            _logger.LogInformation($"Adding a product to customer: {customerId}'s wishlist ");
            var wishlistItem = new Wishlist
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                ProductId = wishlistCreateDto.ProductId
            };
            _repositoryManager.WishlistRepository.CreateWishlist(wishlistItem);
            await _repositoryManager.SaveAsync();
            
            _logger.LogInformation($"Fetching product with ID: {wishlistCreateDto.ProductId}");
            var product =
                await _repositoryManager.ProductRepository.GetProductByIdAsync(wishlistCreateDto.ProductId);

            _logger.LogInformation($"Returning wishlist item with ID: {wishlistItem.Id}.");
            var wishlistItemToReturn = new WishlistReadDto
            (
                Id: wishlistItem.Id,
                CustomerId: wishlistItem.CustomerId,
                ProductId: wishlistCreateDto.ProductId,
                ProductName: product!.Name,
                ProductDescription: product.Description
            );

            return wishlistItemToReturn;
        }

        public async Task DeleteWishlistItemAsync(Guid id)
        {
            _logger.LogInformation($"Fetching wishlist with ID: {id} to delete");
            var wishlistToDelete = 
                await _repositoryManager.WishlistRepository.GetWishlistByIdAsync(id);
            if (wishlistToDelete is null)
                throw new NotFoundException("Wishlist item", id);
            
            _logger.LogInformation($"Deleting wishlist with ID: {id}");
            _repositoryManager.WishlistRepository.DeleteWishlist(wishlistToDelete);

            await _repositoryManager.SaveAsync();
        }
    }
}