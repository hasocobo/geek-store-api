namespace StoreApi.Common.DataTransferObjects.Categories;

public record CategoryReadDto
{
    public string Name { get; set; }

    public Guid Id { get; set; }

    public Guid? ParentCategoryId { get; set; }

    public IEnumerable<CategoryReadDto> SubCategories { get; set; } = new List<CategoryReadDto>();
};