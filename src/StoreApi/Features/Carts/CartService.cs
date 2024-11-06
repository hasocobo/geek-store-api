using StoreApi.Common.DataTransferObjects.Carts;
using StoreApi.Entities;
using StoreApi.Entities.Exceptions;

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

        public async Task<IEnumerable<CartReadDto>> GetCartsAsync()
        {
            _logger.LogInformation("Getting all carts");
            var cartItems =
                await _repositoryManager.CartRepository.GetCartsAsync();

            _logger.LogInformation("Returning all carts");
            var cartItemsToReturn =
                cartItems.Select(ci =>
                    new CartReadDto
                    (
                        Id: ci.Id,
                        ProductId: ci.ProductId,
                        Quantity: ci.Quantity,
                        ProductName: ci.Product.Name,
                        UnitPrice: ci.Product.Price
                    )
                );
            return cartItemsToReturn;
        }

        public async Task<CartReadDto> GetCartByIdAsync(Guid cartId)
        {
            _logger.LogInformation($"Getting cart with Id: {cartId}");
            var cartItem =
                await _repositoryManager.CartRepository.GetCartByIdAsync(cartId);
            if (cartItem is null)
                throw new NotFoundException("Cart item", cartId);

            _logger.LogInformation($"Returning cart with Id: {cartId}");
            var cartItemToReturn = new CartReadDto
            (
                Id: cartItem.Id,
                ProductId: cartItem.ProductId,
                Quantity: cartItem.Quantity,
                ProductName: cartItem.Product.Name,
                UnitPrice: cartItem.Product.Price
            );
            return cartItemToReturn;
        }

        public async Task<IEnumerable<CartReadDto>> GetCartsByCustomerIdAsync(Guid customerId)
        {
            if (!await _repositoryManager.CustomerRepository.CheckIfCustomerExists(customerId))
                throw new NotFoundException("Customer", customerId);

            _logger.LogInformation($"Getting carts for Customer: {customerId}");
            var cartItems =
                await _repositoryManager.CartRepository.GetCartsByCustomerIdAsync(customerId);

            _logger.LogInformation($"Returning carts for Customer: {customerId}");
            var cartItemsToReturn =
                cartItems.Select(ci =>
                    new CartReadDto
                    (
                        Id: ci.Id,
                        ProductId: ci.ProductId,
                        Quantity: ci.Quantity,
                        ProductName: ci.Product.Name,
                        UnitPrice: ci.Product.Price
                    )
                );
            return cartItemsToReturn;
        }

        public async Task<CartReadDto> CreateCartForCustomerAsync(Guid customerId, CartCreateDto cartCreateDto)
        {
            if (!await _repositoryManager.CustomerRepository.CheckIfCustomerExists(customerId))
                throw new NotFoundException("Customer", customerId);
            
            if (!await _repositoryManager.ProductRepository.CheckIfProductExists(cartCreateDto.ProductId))
                throw new NotFoundException("Product", cartCreateDto.ProductId);

            _logger.LogInformation($"Creating cart for customer: {customerId}.");
            var cartItem = new Cart
            {
                Id = Guid.NewGuid(),
                ProductId = cartCreateDto.ProductId,
                Quantity = cartCreateDto.Quantity,
                CustomerId = customerId
            };
            _repositoryManager.CartRepository.AddToCart(cartItem);

            _logger.LogInformation($"Cart creation is successful for customer: {customerId}, saving to database.");
            await _repositoryManager.SaveAsync();

            _logger.LogInformation($"Cart saved successfully, returning read-only cart object.");
            var product = 
                await _repositoryManager.ProductRepository.GetProductByIdAsync(cartCreateDto.ProductId);
            
            var cartToReturn = new CartReadDto
            (
                Id: cartItem.Id,
                ProductId: product.Id,
                Quantity: cartItem.Quantity,
                ProductName: product.Name,
                UnitPrice: product.Price
            );
            return cartToReturn;
        }

        public async Task UpdateCartAsync(Guid customerId, Guid id, CartUpdateDto cartUpdateDto)
        {
            if (!await _repositoryManager.CustomerRepository.CheckIfCustomerExists(customerId))
                throw new NotFoundException("Customer", customerId);

            _logger.LogInformation($"Fetching cart item: {id} for customer: {customerId} to update.");
            var cartToUpdate =
                await _repositoryManager.CartRepository.GetCartByIdAsync(id);
            if (cartToUpdate is null)
                throw new NotFoundException("Cart item", id);

            _logger.LogInformation($"Updating cart item: {id} for customer: {customerId}.");
            cartToUpdate.Quantity = cartUpdateDto.Quantity;
            _repositoryManager.CartRepository.UpdateCart(cartToUpdate);

            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteCartAsync(Guid id)
        {
            _logger.LogInformation($"Fetching cart to delete with Id: {id}");
            var cartItem =
                await _repositoryManager.CartRepository.GetCartByIdAsync(id);
            if (cartItem is null)
                throw new NotFoundException("Cart item", id);

            _logger.LogInformation($"Deleting cart with Id: {id}");
            _repositoryManager.CartRepository.DeleteCart(cartItem);

            await _repositoryManager.SaveAsync();
        }
    }
}