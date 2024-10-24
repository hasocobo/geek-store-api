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
    }
}
