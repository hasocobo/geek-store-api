using StoreApi.Entities;

namespace StoreApi.Features.Categories
{
    public interface ICategoryRepository
    {
        Task<bool> CheckIfCategoryExists(Guid categoryId);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(Guid categoryId);
        
        void CreateCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
    }
}
