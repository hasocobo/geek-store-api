using Microsoft.EntityFrameworkCore;
using StoreApi.Entities;
using StoreApi.Infrastructure;

namespace StoreApi.Features.Categories
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(StoreContext storeContext) : base(storeContext)
        {
        }


        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(Guid categoryId)
        {
            var category = await FindByCondition(category =>
                    category.Id.Equals(categoryId))
                .SingleOrDefaultAsync();
            return category ?? throw new InvalidOperationException();
        }

        public void CreateCategory(Category category)
        {
            Create(category);
        }
    }
}