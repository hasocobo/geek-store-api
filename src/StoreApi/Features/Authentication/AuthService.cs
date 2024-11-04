using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StoreApi.Common.DataTransferObjects.Authentication;
using StoreApi.Entities;

namespace StoreApi.Features.Authentication;

public class AuthService : IAuthService
{
    private User? _user;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AuthService> _logger;
    private readonly IRepositoryManager _repositoryManager;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AuthService> logger,
        IRepositoryManager repositoryManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _repositoryManager = repositoryManager;
        _configuration = configuration;
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

    public async Task<bool> ValidateUserAsync(UserAuthenticationDto userAuthenticationDto)
    {
        _logger.LogInformation("Validating user credentials");
        _user = await _userManager.FindByNameAsync(userAuthenticationDto.Username);

        var result = await _userManager.CheckPasswordAsync(_user!, userAuthenticationDto.Password);

        if (!result)
        {
            _logger.LogWarning("User credentials are invalid");
        }

        return result;
    }

    public async Task<string> CreateTokenAsync()
    {
        if (_user == null) throw new UnauthorizedAccessException();

        _logger.LogInformation("Registering claims");
        var claims = new List<Claim>
            { new Claim(ClaimTypes.NameIdentifier, _user.Id), new Claim(ClaimTypes.Name, _user.UserName) };
        var roles = await _userManager.GetRolesAsync(_user);
        if (roles.Any())
        {
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        }

        _logger.LogInformation("Creating signing credentials");
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("DOTNETJWTSECRET") ?? throw new
            InvalidOperationException());
        var secret = new SymmetricSecurityKey(key);
        var signingCredentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

        _logger.LogInformation("Creating token");
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var jwtToken = new JwtSecurityToken
        (
            issuer: jwtSettings["validIssuer"],
            audience: jwtSettings["validAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}