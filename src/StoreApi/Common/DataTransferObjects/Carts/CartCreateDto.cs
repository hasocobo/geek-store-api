namespace StoreApi.Common.DataTransferObjects.Carts;

public record CartCreateDto(Guid ProductId, int Quantity);