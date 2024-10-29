using StoreApi.Features.Carts;
using StoreApi.Features.Categories;
using StoreApi.Features.Customers;
using StoreApi.Features.Orders;
using StoreApi.Features.Products;
using StoreApi.Features.Wishlists;
using StoreApi.Infrastructure;

namespace StoreApi.Features
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly StoreContext _storeContext;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IWishlistRepository _wishlistRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICartRepository _cartRepository;

        public RepositoryManager(StoreContext storeContext, IProductRepository productRepository,
            IOrderRepository orderRepository, ICustomerRepository customerRepository,
            IWishlistRepository wishlistRepository, ICategoryRepository categoryRepository,
            ICartRepository cartRepository)
        {
            _storeContext = storeContext;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _wishlistRepository = wishlistRepository;
            _categoryRepository = categoryRepository;
            _cartRepository = cartRepository;
        }

        public IProductRepository ProductRepository => _productRepository;
        public IOrderRepository OrderRepository => _orderRepository;
        public ICartRepository CartRepository => _cartRepository;
        public ICustomerRepository CustomerRepository => _customerRepository;
        public IWishlistRepository WishlistRepository => _wishlistRepository;
        public ICategoryRepository CategoryRepository => _categoryRepository;

        public async Task SaveAsync() => await _storeContext.SaveChangesAsync();
    }
}