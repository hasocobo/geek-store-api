using StoreApi.Entities;

namespace StoreApi.Common.DataTransferObjects.Orders;

public record OrderCreateDto
{
    public Guid? ShipmentId { get; init; }
    public Guid? PaymentId { get; init; }
    public ICollection<OrderItem>? OrderItems { get; init; }
}