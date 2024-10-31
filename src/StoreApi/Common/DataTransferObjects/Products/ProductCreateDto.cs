namespace StoreApi.Common.DataTransferObjects.Products;

public record ProductCreateDto(string Name, decimal Price, string? Description);