using StoreApi.Common.ValidationAttributes;

namespace StoreApi.Common.DataTransferObjects.Wishlists;

public record WishlistReadDto(
    Guid Id,
    Guid ProductId,
    Guid CustomerId,
    string? ProductName,
    string? ProductDescription);