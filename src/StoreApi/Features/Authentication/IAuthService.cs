using Microsoft.AspNetCore.Identity;
using StoreApi.Common.DataTransferObjects.Authentication;

namespace StoreApi.Features.Authentication;

public interface IAuthService
{
    Task<UserDetails> GetUserByIdAsync(string id);
    Task<IEnumerable<UserDetails>> GetUsersAsync();
    Task<(IdentityResult, UserDetails)> RegisterUserAndCustomerAsync(UserRegistrationDto userRegistrationDto);
    Task<string> CreateTokenAsync();
    Task<bool> ValidateUserAsync(UserAuthenticationDto userAuthenticationDto);

}