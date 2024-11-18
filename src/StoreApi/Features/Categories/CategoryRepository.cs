using Microsoft.EntityFrameworkCore;
using StoreApi.Entities;
using StoreApi.Entities.Exceptions;
using StoreApi.Infrastructure;

namespace StoreApi.Features.Categories
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(StoreContext storeContext) : base(storeContext)
        {
        }

        public async Task<bool> CheckIfCategoryExists(Guid categoryId)
        {
            return await Exists(c => c.Id.Equals(categoryId));
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await FindAll()
                .Include(c => c.SubCategories)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid categoryId)
        {
            return await FindByCondition(category =>
                    category.Id.Equals(categoryId))
                .Include(c => c.SubCategories)
                .SingleOrDefaultAsync();
        }

        public async Task<Category?> GetCategoryWithoutSubCategoriesByIdAsync(Guid categoryId)
        {
            return await FindByCondition(category =>
                    category.Id.Equals(categoryId))
                .SingleOrDefaultAsync();
        }

        public void CreateCategory(Category category)
        {
            Create(category);
        }

        public void UpdateCategory(Category category)
        {
            Update(category);
        }

        public void DeleteCategory(Category category)
        {
            Delete(category);
        }
    }
}