using StoreApi.Features.Authentication;
using StoreApi.Features.Carts;
using StoreApi.Features.Categories;
using StoreApi.Features.Customers;
using StoreApi.Features.Orders;
using StoreApi.Features.Products;
using StoreApi.Features.Wishlists;

namespace StoreApi.Features
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IWishlistService _wishlistService;
        private readonly ICategoryService _categoryService;
        private readonly ICustomerService _customerService;
        private readonly IAuthService _authService;

        public ServiceManager(ICartService cartService,
            IOrderService orderService, IProductService productService, IWishlistService wishlistService,
            ICategoryService categoryService, ICustomerService customerService, IAuthService authService)
        {
            _orderService = orderService;
            _productService = productService;
            _wishlistService = wishlistService;
            _categoryService = categoryService;
            _customerService = customerService;
            _cartService = cartService;
            _authService = authService;
        }

        public IProductService ProductService => _productService;
        public IOrderService OrderService => _orderService;
        public ICustomerService CustomerService => _customerService;
        public ICartService CartService => _cartService;
        public IWishlistService WishlistService => _wishlistService;
        public ICategoryService CategoryService => _categoryService;
        public IAuthService AuthService => _authService;
    }
}