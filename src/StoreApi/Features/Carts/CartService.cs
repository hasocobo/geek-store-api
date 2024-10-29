using StoreApi.Common.DataTransferObjects.Carts;
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

        public async Task<CartReadDto> CreateCartForCustomerAsync(Guid customerId, CartCreateDto cartCreateDto)
        {
            _logger.LogInformation($"Creating cart for customer: {customerId}");
            var cartItem = new Cart
            {
                Id = Guid.NewGuid(),
                ProductId = cartCreateDto.ProductId,
                Quantity = cartCreateDto.Quantity,
                CustomerId = customerId
            };
            _repositoryManager.CartRepository.CreateCartForCustomer(customerId, cartItem);
            
            _logger.LogInformation($"Cart creation is successful for customer: {customerId}, saving to database");
            await _repositoryManager.SaveAsync();

            var product = await _repositoryManager.ProductRepository.GetProductByIdAsync(cartCreateDto.ProductId);
            
            var cartToReturn = new CartReadDto
            (
                ProductId: product.Id,
                Quantity: cartCreateDto.Quantity,
                ProductName: product.Name,
                UnitPrice: product.Price
            );
            return cartToReturn;
        }
    }
}