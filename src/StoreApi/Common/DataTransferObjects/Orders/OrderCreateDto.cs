using StoreApi.Entities;

namespace StoreApi.Common.DataTransferObjects.Orders;

public record OrderCreateDto(Guid? ShipmentId, Guid? PaymentId, ICollection<OrderItem>? OrderItems);