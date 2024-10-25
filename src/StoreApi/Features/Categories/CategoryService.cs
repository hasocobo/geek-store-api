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

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _repositoryManager.CategoryRepository.GetAllCategoriesAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(Guid id)
        {
            return await _repositoryManager.CategoryRepository.GetCategoryByIdAsync(id);
        }
        
    }
}
