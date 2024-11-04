using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreApi.Common.DataTransferObjects.Authentication;
using StoreApi.Entities;

namespace StoreApi.Features.Authentication;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AuthService> _logger;
    private readonly IRepositoryManager _repositoryManager;

    public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AuthService> logger,
        IRepositoryManager repositoryManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _repositoryManager = repositoryManager;
    }

    public async Task<UserDetails> GetUserByIdAsync(string id)
    {
        _logger.LogInformation($"Getting user details for user with id: {id}");
        var user = await _userManager.FindByIdAsync(id);

        _logger.LogInformation($"Returning user details for user with id: {id}");
        var userDetails = new UserDetails
        (
            Id: user.Id,
            FirstName: user.FirstName,
            LastName: user.LastName,
            DateOfBirth: user.DateOfBirth,
            Email: user.Email,
            UserName: user.UserName
        );

        return userDetails;
    }

    public async Task<IEnumerable<UserDetails>> GetUsersAsync()
    {
        _logger.LogInformation($"Getting all users");
        var users = await _userManager.Users.ToListAsync();

        _logger.LogInformation($"Returning all users");
        var usersToReturn =
            users.Select(u => new UserDetails(Id: u.Id, FirstName: u.FirstName, LastName: u.LastName,
                UserName: u.UserName, DateOfBirth: u.DateOfBirth, Email: u.Email));
        
        return usersToReturn;
    }

    public async Task<(IdentityResult, UserDetails)> RegisterUserAndCustomerAsync(UserRegistrationDto registerDto)
    {
        _logger.LogInformation("Registering user");
        var newUser = new User
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            UserName = registerDto.UserName,
            DateOfBirth = registerDto.DateOfBirth ?? DateTime.MinValue,
        };

        var result = await _userManager.CreateAsync(newUser, registerDto.Password);

        if (result.Succeeded)
        {
            _logger.LogInformation("User registration successful");
            if (registerDto.Roles != null)
            {
                _logger.LogInformation("Registering roles to user");
                await _userManager.AddToRolesAsync(newUser, registerDto.Roles);
            }

            _logger.LogInformation("Registering customer");
            var customer = new Customer
            {
                UserId = newUser.Id,
                Id = Guid.NewGuid(),
                Address = registerDto.Address,
                PhoneNumber = registerDto.PhoneNumber,
            };

            _repositoryManager.CustomerRepository.CreateCustomer(customer);
            await _repositoryManager.SaveAsync();
        }

        _logger.LogInformation("Returning user details.");
        var userDetails = new UserDetails
        (
            Id: newUser.Id,
            FirstName: newUser.FirstName,
            LastName: newUser.LastName,
            DateOfBirth: newUser.DateOfBirth,
            Email: newUser.Email,
            UserName: newUser.UserName
        );

        return (result, userDetails);
    }
}