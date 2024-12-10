using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using StoreApi.Common.DataTransferObjects.Categories;
using StoreApi.Entities;
using StoreApi.Entities.Exceptions;
using StoreApi.Features;
using StoreApi.Features.Categories;

namespace StoreApiTests.CategoryTests;

public class CategoryUnitTests
{
    private readonly Mock<IRepositoryManager> _repositoryManagerMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly ICategoryService _categoryService;

    public CategoryUnitTests()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _repositoryManagerMock = new Mock<IRepositoryManager>();

        _repositoryManagerMock
            .Setup(repoManager => repoManager.CategoryRepository)
            .Returns(_categoryRepositoryMock.Object);

        _repositoryManagerMock
            .Setup(repoManager => repoManager.SaveAsync())
            .Returns(Task.CompletedTask);

        _categoryService = new CategoryService(_repositoryManagerMock.Object, NullLogger<CategoryService>.Instance);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_ValidInput_ReturnsCategory()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        _categoryRepositoryMock.Setup(
                repo
                    => repo.GetCategoryByIdAsync(categoryId))
            .ReturnsAsync(new Category { Id = categoryId, Name = "Test Category" });

        var categoryExpected = new CategoryReadDto { Id = categoryId, Name = "Test Category" };

        // Act
        var result = await _categoryService.GetCategoryByIdAsync(categoryId);

        // Assert
        Assert.Equal(categoryExpected.Id, result.Id);
        Assert.Equal(categoryExpected.Name, result.Name);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_InvalidId_ThrowsNotFoundException()
    {
        // Arrange
        var invalidId = Guid.Empty;
        _categoryRepositoryMock.Setup(repo =>
            repo.GetCategoryByIdAsync(invalidId)).ReturnsAsync(null as Category);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _categoryService.GetCategoryByIdAsync(invalidId));
    }

    [Fact]
    public async Task GetCategoriesAsync_ReturnsCategories()
    {
        // Arrange
        var categories = new List<Category>()
        {
            new Category { Id = Guid.NewGuid(), Name = "Test Category 1" },
            new Category { Id = Guid.NewGuid(), Name = "Test Category 1" },
            new Category { Id = Guid.NewGuid(), Name = "Test Category 3" },
        };
        _categoryRepositoryMock.Setup(repo =>
            repo.GetAllCategoriesAsync()).ReturnsAsync(categories);

        // Act
        var result = await _categoryService.GetCategoriesAsync();

        // Assert
        Assert.Equal(categories.Count, result.Count());
    }

    [Fact]
    public async Task CreateCategoryAsync_ValidInput_ReturnsCreatedCategory()
    {
        // Arrange
        var newCategory = new CategoryCreateDto(Name: "New Category", ParentCategoryId: null);
        var createdCategory = new CategoryReadDto
            { Id = Guid.NewGuid(), Name = "New Category", ParentCategoryId = null };

        _categoryRepositoryMock.Setup(repo
            => repo.CreateCategory(It.IsAny<Category>()));

        // Act
        var result = await _categoryService.CreateCategoryAsync(newCategory);

        // Assert
        Assert.Equal(createdCategory.Name, result.Name);
        Assert.Null(result.ParentCategoryId);
        _categoryRepositoryMock.Verify(repo => repo.CreateCategory(It.IsAny<Category>()), Times.Once);
        _repositoryManagerMock.Verify(rm => rm.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateCategoryAsync_WithParentCategory_ValidInput_ReturnsCreatedCategory()
    {
        // Arrange
        var parentCategoryId = Guid.NewGuid();
        var newCategory = new CategoryCreateDto(Name: "New Category", ParentCategoryId: parentCategoryId);
        var createdCategory = new CategoryReadDto
            { Id = Guid.NewGuid(), Name = "New Category", ParentCategoryId = parentCategoryId };

        _categoryRepositoryMock.Setup(repo
            => repo.CreateCategory(It.IsAny<Category>()));

        _categoryRepositoryMock.Setup(repo
            => repo.CheckIfCategoryExists(parentCategoryId)).ReturnsAsync(true);

        // Act
        var result = await _categoryService.CreateCategoryAsync(newCategory);

        // Assert
        Assert.Equal(createdCategory.Name, result.Name);
        Assert.Equal(createdCategory.ParentCategoryId, result.ParentCategoryId);
        _categoryRepositoryMock.Verify(repo => repo.CreateCategory(It.IsAny<Category>()), Times.Once);
        _repositoryManagerMock.Verify(rm => rm.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateCategoryAsync_WithParentCategory_InvalidInput_ThrowsNotFoundException()
    {
        // Arrange
        var parentCategoryId = Guid.NewGuid();
        var newCategory = new CategoryCreateDto(Name: "New Category", ParentCategoryId: parentCategoryId);
        var createdCategory = new CategoryReadDto
            { Id = Guid.NewGuid(), Name = "New Category", ParentCategoryId = parentCategoryId };

        _categoryRepositoryMock.Setup(repo
            => repo.CreateCategory(It.IsAny<Category>()));

        _categoryRepositoryMock.Setup(repo
            => repo.CheckIfCategoryExists(parentCategoryId)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _categoryService.CreateCategoryAsync(newCategory));
    }

    [Fact]
    public async Task UpdateCategoryAsync_ValidId_CallsRepository()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var oldCategory = new Category { Id = categoryId, Name = "Old Category" };
        var categoryUpdateDto = new CategoryUpdateDto(Name: "Updated Category");
        _categoryRepositoryMock.Setup(repo => repo.GetCategoryByIdAsync(categoryId))
            .ReturnsAsync(oldCategory);

        // Act & Assert
        await _categoryService.UpdateCategoryAsync(categoryId, categoryUpdateDto);
        Assert.Equal(oldCategory.Name, categoryUpdateDto.Name);
        _categoryRepositoryMock.Verify(repo => repo.UpdateCategory(oldCategory), Times.Once);
    }

    [Fact]
    public async Task UpdateCategoryAsync_InvalidId_ThrowsNotFoundException()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var categoryUpdateDto = new CategoryUpdateDto(Name: "Updated Category");
        _categoryRepositoryMock.Setup(repo => repo.GetCategoryByIdAsync(categoryId))
            .ReturnsAsync(null as Category);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _categoryService.UpdateCategoryAsync(categoryId, categoryUpdateDto));
    }

    [Fact]
    public async Task DeleteCategoryAsync_ValidId_CallsRepository()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var categoryToDelete = new Category { Id = categoryId, Name = "Category To Delete" };
        _categoryRepositoryMock.Setup(repo
            => repo.GetCategoryByIdAsync(categoryId)).ReturnsAsync(categoryToDelete);

        // Act
        await _categoryService.DeleteCategoryAsync(categoryId);

        // Assert
        _categoryRepositoryMock.Verify(repo => repo.DeleteCategory(categoryToDelete), Times.Once);
    }

    [Fact]
    public async Task DeleteCategoryAsync_InvalidId_ThrowsNotFoundException()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        _categoryRepositoryMock.Setup(repo
            => repo.GetCategoryByIdAsync(categoryId)).ReturnsAsync(null as Category);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _categoryService.DeleteCategoryAsync(categoryId));
    }
}