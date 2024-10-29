namespace StoreApi.Common.DataTransferObjects.Carts;

public record CartReadDto(Guid ProductId, int Quantity, string? ProductName, decimal UnitPrice);