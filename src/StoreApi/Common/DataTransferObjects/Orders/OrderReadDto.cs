using StoreApi.Common.DataTransferObjects.Products;

namespace StoreApi.Common.DataTransferObjects.Orders;

public record OrderReadDto(
    Guid Id,
    DateTime Date,
    decimal TotalPrice,
    Guid CustomerId,
    ICollection<OrderItemReadDto> OrderItems);