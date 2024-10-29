using StoreApi.Entities;

namespace StoreApi.Features.Categories
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(Guid id);
        
        Task CreateCategoryAsync(Category category);
    }
}
