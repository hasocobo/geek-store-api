namespace StoreApi.Common.DataTransferObjects.Products;

public record ProductReadDto(Guid Id, string Name, decimal Price, string? Description, string CategoryName);