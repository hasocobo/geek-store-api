using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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
            var query = FindAll()
                .Include(p => p.Category)
                .AsNoTracking(); // no need for tracking because it's meant to be read-only

            if (!string.IsNullOrWhiteSpace(queryParameters.SearchTerm))
            {
                var keywords = queryParameters.SearchTerm
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(k => k.Trim().ToLower())
                    .ToArray();

                // search query for each keyword. 
                query = query.Where(p =>
                    keywords.Any(keyword =>
                        p.Name.ToLower().Contains(keyword) ||
                        (p.Description != null &&
                         p.Description.ToLower().Contains(keyword))
                    ));
            }

            if (queryParameters.Filters.Count > 0)
            {
                foreach (var filter in queryParameters.Filters)
                {
                    query = filter.Key.ToLower() switch
                    {
                        "pricemin" => query.Where(p => p.Price > int.Parse(filter.Value)),
                        "pricemax" => query.Where(p => p.Price < int.Parse(filter.Value)),
                        _ => query
                    };
                }
            }

            if (!string.IsNullOrWhiteSpace(queryParameters.SortBy))
            {
                query = queryParameters.SortBy.ToLower() switch
                {
                    "name" => queryParameters.SortDescending
                        ? query.OrderByDescending(p => p.Name)
                        : query.OrderBy(p => p.Name),

                    "price" => queryParameters.SortDescending
                        ? query.OrderByDescending(p => p.Price)
                        : query.OrderBy(p => p.Price),

                    _ => throw new BadHttpRequestException($"Unsupported sort field: {queryParameters.SortBy}")
                };
            }

            var totalRecordSize = await query.CountAsync();

            var products = await query
                .Skip(queryParameters.PageSize * (queryParameters.PageNumber - 1))
                .Take(queryParameters.PageSize)
                .ToListAsync();

            var metadata = new Metadata
            {
                CurrentPage = queryParameters.PageNumber,
                TotalPages = (int)Math.Ceiling(totalRecordSize / (double)queryParameters.PageSize),
                PageSize = queryParameters.PageSize,
                TotalRecords = totalRecordSize
            };

            return (products, metadata);
        }


        public async Task<(IEnumerable<Product>, Metadata)> GetProductsByCategoryIdAsync(Guid categoryId,
            QueryParameters queryParameters)
        {
            var query = FindByCondition(p => p.CategoryId.Equals(categoryId))
                .Include(p => p.Category)
                .AsNoTracking();
            
            // apply searching
            if (!string.IsNullOrWhiteSpace(queryParameters.SearchTerm))
            {
                var keywords = queryParameters.SearchTerm
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(k => k.Trim().ToLower())
                    .ToArray();

                // search query for each keyword. 
                query = query.Where(p =>
                    keywords.Any(keyword =>
                        p.Name.ToLower().Contains(keyword) ||
                        (p.Description != null &&
                         p.Description.ToLower().Contains(keyword))
                    ));
            }

            // apply filtering 
            if (queryParameters.Filters.Count > 0)
            {
                foreach (var filter in queryParameters.Filters)
                {
                    query = filter.Key.ToLower() switch
                    {
                        "pricemin" => query.Where(p => p.Price > int.Parse(filter.Value)),
                        "pricemax" => query.Where(p => p.Price < int.Parse(filter.Value)),
                        _ => query
                    };
                }
            }

            // apply sorting
            if (!string.IsNullOrWhiteSpace(queryParameters.SortBy))
            {
                query = queryParameters.SortBy.ToLower() switch
                {
                    "name" => queryParameters.SortDescending
                        ? query.OrderByDescending(p => p.Name)
                        : query.OrderBy(p => p.Name),

                    "price" => queryParameters.SortDescending
                        ? query.OrderByDescending(p => p.Price)
                        : query.OrderBy(p => p.Price),

                    _ => throw new BadHttpRequestException($"Unsupported sort field: {queryParameters.SortBy}")
                };
            }

            var totalRecordSize = await query.CountAsync();

            var products = await query
                .Skip(queryParameters.PageSize * (queryParameters.PageNumber - 1))
                .Take(queryParameters.PageSize)
                .ToListAsync();

            var metadata = new Metadata
            {
                CurrentPage = queryParameters.PageNumber,
                TotalPages = (int)Math.Ceiling(totalRecordSize / (double)queryParameters.PageSize),
                PageSize = queryParameters.PageSize,
                TotalRecords = totalRecordSize
            };

            return (products, metadata);
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