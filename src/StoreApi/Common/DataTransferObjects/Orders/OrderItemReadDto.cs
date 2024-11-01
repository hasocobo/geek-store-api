namespace StoreApi.Common.DataTransferObjects.Orders;

public record OrderItemReadDto(Guid Id, Guid ProductId, decimal UnitPrice, string ProductName, int Quantity);