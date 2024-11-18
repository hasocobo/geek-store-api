using StoreApi.Common.ValidationAttributes;

namespace StoreApi.Common.DataTransferObjects.Categories;

public record CategoryCreateDto(string Name, Guid? ParentCategoryId);