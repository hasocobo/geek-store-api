using StoreApi.Common.ValidationAttributes;

namespace StoreApi.Common.DataTransferObjects.Products;

public record ProductUpdateDto
{
    public string? Name;
    public decimal? Price;
    public string? Description;
    public string? Sku;
    
    [NotEmptyGuid] 
    public Guid CategoryId;
};