using StoreApi.Common.DataTransferObjects.Categories;
using StoreApi.Entities;
using StoreApi.Entities.Exceptions;

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

        private async Task GetCategoryPath(Category category)
        {
        }

        public async Task<IEnumerable<CategoryReadDto>> GetCategoriesAsync()
        {
            _logger.LogInformation("Getting all categories");
            var categories =
                await _repositoryManager.CategoryRepository.GetAllCategoriesAsync();

            var categoriesToReturn = categories
                .Select(c => new CategoryReadDto { Id = c.Id, Name = c.Name, ParentCategoryId = c.ParentCategoryId });

            return categoriesToReturn;
        }

        public async Task<CategoryReadDto> GetCategoryByIdAsync(Guid id)
        {
            _logger.LogInformation($"Getting category with id: {id}");
            var category =
                await _repositoryManager.CategoryRepository.GetCategoryByIdAsync(id);
            if (category is null)
                throw new NotFoundException("Category", id);

            return new CategoryReadDto
            {
                Id = category.Id,
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId,
                SubCategories = category.SubCategories.Select
                    (sc => new CategoryReadDto { Id = sc.Id, Name = sc.Name })
            };
        }

        public async Task<CategoryReadDto> CreateCategoryAsync(CategoryCreateDto categoryCreateDto)
        {
            if (categoryCreateDto.ParentCategoryId != null &&
                !await _repositoryManager.CategoryRepository
                    .CheckIfCategoryExists(categoryCreateDto.ParentCategoryId.Value))
            {
                throw new NotFoundException("Parent Category", categoryCreateDto.ParentCategoryId.Value);
            }

            _logger.LogInformation("Creating new category");
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = categoryCreateDto.Name,
                ParentCategoryId = categoryCreateDto.ParentCategoryId
            };
            _repositoryManager.CategoryRepository.CreateCategory(category);
            await _repositoryManager.SaveAsync();

            _logger.LogInformation("Category creation successful, returning category data transfer object.");
            var categoryToReturn = new CategoryReadDto
                { Id = category.Id, Name = category.Name, ParentCategoryId = category.ParentCategoryId };

            return categoryToReturn;
        }

        public async Task UpdateCategoryAsync(Guid id, CategoryUpdateDto categoryUpdateDto)
        {
            _logger.LogInformation($"Fetching category with id: {id} to update");
            var categoryToUpdate =
                await _repositoryManager.CategoryRepository.GetCategoryByIdAsync(id);
            if (categoryToUpdate is null)
                throw new NotFoundException("Category", id);

            _logger.LogInformation($"Updating category with id: {id}");
            categoryToUpdate.Name = categoryUpdateDto.Name;
            _repositoryManager.CategoryRepository.UpdateCategory(categoryToUpdate);

            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            _logger.LogInformation($"Fetching category with id: {id} to delete");
            var categoryToDelete =
                await _repositoryManager.CategoryRepository.GetCategoryByIdAsync(id);
            if (categoryToDelete is null)
                throw new NotFoundException("Category", id);

            _logger.LogInformation($"Deleting category with id: {id}");
            _repositoryManager.CategoryRepository.DeleteCategory(categoryToDelete);

            await _repositoryManager.SaveAsync();
        }
    }
}