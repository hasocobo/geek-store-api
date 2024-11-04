namespace StoreApi.Common.DataTransferObjects.Authentication;

public record UserRegistrationDto(
    string FirstName,
    string LastName,
    string UserName,
    string Password,
    string Email,
    string PhoneNumber,
    DateTime? DateOfBirth,
    string? Address,
    ICollection<string>? Roles);