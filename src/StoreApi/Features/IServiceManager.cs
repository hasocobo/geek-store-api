using StoreApi.Features.Authentication;
using StoreApi.Features.Carts;
using StoreApi.Features.Categories;
using StoreApi.Features.Customers;
using StoreApi.Features.Orders;
using StoreApi.Features.Products;
using StoreApi.Features.Wishlists;

namespace StoreApi.Features
{
    public interface IServiceManager
    {
        IProductService ProductService { get; }
        IOrderService OrderService { get; }
        ICustomerService CustomerService { get; }
        ICartService CartService { get; }
        IWishlistService WishlistService { get; }
        ICategoryService CategoryService { get; }
        IAuthService AuthService { get; }
    }
}