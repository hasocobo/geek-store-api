using Microsoft.EntityFrameworkCore;
using StoreApi.Common.QueryFeatures;
using StoreApi.Entities;
using StoreApi.Infrastructure;

namespace StoreApi.Features.Products
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(StoreContext storeContext) : base(storeContext)
        {
        }

        public async Task<bool> CheckIfProductExists(Guid id)
        {
            return await Exists(p => p.Id.Equals(id));
        }

        public async Task<(IEnumerable<Product>, Metadata)> GetProductsAsync(QueryParameters queryParameters)
        {
            var products = await FindAll()
                .Skip(queryParameters.PageSize * (queryParameters.PageNumber - 1))
                .Take(queryParameters.PageSize)
                .Include(p => p.Category)
                .ToListAsync();

            var totalRecordSize = await FindAll().CountAsync();
            
            var metadata = new Metadata
            {
                CurrentPage = queryParameters.PageNumber,
                TotalPages = (int)Math.Ceiling(totalRecordSize / (double)queryParameters.PageSize),
                PageSize = queryParameters.PageSize,
                TotalRecords = totalRecordSize
            };
            
            return (products, metadata);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(Guid categoryId)
        {
            return await FindByCondition(p => p.CategoryId.Equals(categoryId))
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(Guid productId)
        {
            return await FindByCondition(p => p.Id.Equals(productId))
                .Include(p => p.Category)
                .SingleOrDefaultAsync();
        }

        public void CreateProduct(Product product)
        {
            Create(product);
        }

        public void UpdateProduct(Product product)
        {
            Update(product);
        }

        public void DeleteProduct(Product product)
        {
            Delete(product);
        }
    }
}