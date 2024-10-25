using StoreApi.Entities;

namespace StoreApi.Features.Categories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(Guid categoryId);
        
        
    }
}
