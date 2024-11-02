namespace StoreApi.Common.DataTransferObjects.Products;

public record ProductUpdateDto(string? Name, decimal? Price, string? Description, string? Sku, Guid? CategoryId);