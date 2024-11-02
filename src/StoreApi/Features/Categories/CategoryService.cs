using StoreApi.Common.DataTransferObjects.Categories;
using StoreApi.Entities;

namespace StoreApi.Features.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(IRepositoryManager repositoryManager, ILogger<CategoryService> logger)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
        }

        public async Task<IEnumerable<CategoryReadDto>> GetCategoriesAsync()
        {
            _logger.LogInformation("Getting all categories");
            var categories = await _repositoryManager.CategoryRepository.GetAllCategoriesAsync();
            var categoriesToReturn = categories
                .Select(c => new CategoryReadDto(Id: c.Id, Name: c.Name));
            return categoriesToReturn;
        }

        public async Task<CategoryReadDto> GetCategoryByIdAsync(Guid id)
        {
            _logger.LogInformation($"Getting category with id: {id}");
            var category = await _repositoryManager.CategoryRepository.GetCategoryByIdAsync(id);
            return new CategoryReadDto(Id: category.Id, Name: category.Name);
        }

        public async Task<CategoryReadDto> CreateCategoryAsync(CategoryCreateDto categoryCreateDto)
        {
            _logger.LogInformation("Creating new category");
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = categoryCreateDto.Name,
            };
            _repositoryManager.CategoryRepository.CreateCategory(category);
            await _repositoryManager.SaveAsync();
            
            _logger.LogInformation("Category creation successful, returning category data transfer object.");
            var categoryToReturn = new CategoryReadDto(Id: category.Id, Name: category.Name);
            return categoryToReturn;
        }

        public async Task UpdateCategoryAsync(Guid id, CategoryUpdateDto categoryUpdateDto)
        {
            _logger.LogInformation($"Fetching category with id: {id} to update");
            var categoryToUpdate = await _repositoryManager.CategoryRepository.GetCategoryByIdAsync(id);
            
            _logger.LogInformation($"Updating category with id: {id}");
            categoryToUpdate.Name = categoryUpdateDto.Name;
            _repositoryManager.CategoryRepository.UpdateCategory(categoryToUpdate);
            
            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            _logger.LogInformation($"Fetching category with id: {id}");
            var categoryToDelete = await _repositoryManager.CategoryRepository.GetCategoryByIdAsync(id);
            
            _logger.LogInformation($"Deleting category with id: {id}");
            _repositoryManager.CategoryRepository.DeleteCategory(categoryToDelete);
            
            await _repositoryManager.SaveAsync();
        }
    }
}