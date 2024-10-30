using StoreApi.Common.DataTransferObjects.Categories;
using StoreApi.Entities;

namespace StoreApi.Features.Categories
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryReadDto>> GetCategoriesAsync();
        Task<CategoryReadDto> GetCategoryByIdAsync(Guid id);
        
        Task<CategoryReadDto> CreateCategoryAsync(CategoryCreateDto category);
        
        Task DeleteCategoryAsync(Guid id);
    }
}
