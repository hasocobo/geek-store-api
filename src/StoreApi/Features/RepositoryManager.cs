using StoreApi.Features.Categories;
using StoreApi.Features.Customers;
using StoreApi.Features.Orders;
using StoreApi.Features.Products;
using StoreApi.Features.Wishlists;
using StoreApi.Infrastructure;

namespace StoreApi.Features
{
    public sealed class RepositoryManager
    {
        private readonly StoreContext _storeContext;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IWishlistRepository _wishlistRepository;
        private readonly ICategoryRepository _categoryRepository;

        public RepositoryManager(StoreContext storeContext)
        {
            _storeContext = storeContext;
            _productRepository = new ProductRepository(storeContext);
            _orderRepository = new OrderRepository(storeContext);
            _customerRepository = new CustomerRepository(storeContext);
            _wishlistRepository = new WishlistRepository(storeContext);
            _categoryRepository = new CategoryRepository(storeContext);
        }

        public IProductRepository ProductRepository => _productRepository;
        public IOrderRepository OrderRepository => _orderRepository;
        public ICustomerRepository CustomerRepository => _customerRepository;
        public IWishlistRepository WishlistRepository => _wishlistRepository;
        public ICategoryRepository CategoryRepository => _categoryRepository;

        public void SaveAsync() => _storeContext.SaveChangesAsync();
    }
}
