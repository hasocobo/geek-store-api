using StoreApi.Common.ValidationAttributes;

namespace StoreApi.Common.DataTransferObjects.Carts;

public record CartCreateDto([NotEmptyGuid] Guid ProductId, int Quantity);