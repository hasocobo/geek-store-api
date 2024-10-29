using Microsoft.EntityFrameworkCore;
using StoreApi.Entities;
using StoreApi.Infrastructure;

namespace StoreApi.Features.Products
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(StoreContext storeContext) : base(storeContext)
        {
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(Guid productId)
        {
            return await FindByCondition(p => p.Id.Equals(productId)).SingleOrDefaultAsync() ?? throw new
                InvalidOperationException();
        }

        public void CreateProduct(Product product)
        {
            Create(product);
        }
    }
}