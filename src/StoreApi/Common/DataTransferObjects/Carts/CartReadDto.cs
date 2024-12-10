namespace StoreApi.Common.DataTransferObjects.Carts;

public record CartReadDto(Guid Id, Guid ProductId, Guid CustomerId, int Quantity, string? ProductName, decimal UnitPrice);