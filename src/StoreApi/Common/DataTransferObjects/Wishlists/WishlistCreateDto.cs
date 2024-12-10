using StoreApi.Common.ValidationAttributes;

namespace StoreApi.Common.DataTransferObjects.Wishlists;

public record WishlistCreateDto([NotEmptyGuid] Guid ProductId); // wishlist service already checks if the product
                                                                // with entered product id exists but
                                                                // still this is useful for throwing exceptions earlier. 