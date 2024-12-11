namespace StoreApi.Common.DataTransferObjects.Products;

public record ProductCreateDto
{
    public string Name { get; init; }
    public decimal Price { get; init; }
    public string? Description { get; init; }
}