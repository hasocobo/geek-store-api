namespace StoreApi.Common.DataTransferObjects.Authentication;

public record UserDetails(
    string Id,
    string FirstName,
    string LastName,
    DateTime? DateOfBirth,
    string UserName,
    string Email
);