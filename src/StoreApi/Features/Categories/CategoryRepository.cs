using StoreApi.Entities;
using StoreApi.Infrastructure;

namespace StoreApi.Features.Categories
{
    public class CategoryRepository : RepositoryBase<Customer>, ICategoryRepository
    {
        public CategoryRepository(StoreContext storeContext) : base(storeContext)
        {
        }
    }
}
