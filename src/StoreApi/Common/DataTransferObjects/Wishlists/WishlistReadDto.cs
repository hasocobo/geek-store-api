namespace StoreApi.Common.DataTransferObjects.Wishlists;

public record WishlistReadDto(Guid Id, Guid ProductId, string ProductName, string? ProductDescription);