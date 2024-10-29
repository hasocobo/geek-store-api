using StoreApi.Features.Carts;
using StoreApi.Features.Categories;
using StoreApi.Features.Customers;
using StoreApi.Features.Orders;
using StoreApi.Features.Products;
using StoreApi.Features.Wishlists;

namespace StoreApi.Features
{
    public interface IRepositoryManager
    {
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IOrderRepository OrderRepository { get; }
        IWishlistRepository WishlistRepository { get; }
        ICartRepository CartRepository { get; }
        ICustomerRepository CustomerRepository { get; }

        Task SaveAsync();
    }
}