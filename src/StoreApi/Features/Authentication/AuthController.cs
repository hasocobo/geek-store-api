using Microsoft.AspNetCore.Mvc;
using StoreApi.Common.DataTransferObjects.Authentication;

namespace StoreApi.Features.Authentication;

[Route("api/v1")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public AuthController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpPost("users")]
    public async Task<ActionResult> RegisterUser([FromBody] UserRegistrationDto registerDto)
    {
        var (result, userToReturn) =
            await _serviceManager.AuthService.RegisterUserAndCustomerAsync(registerDto);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return CreatedAtAction(nameof(GetUserById), new { id = userToReturn.Id }, userToReturn);
    }

    [HttpPost("authentication/login")]
    public async Task<ActionResult> Authenticate([FromBody] UserAuthenticationDto userAuthenticationDto)
    {
        if (!await _serviceManager.AuthService.ValidateUserAsync(userAuthenticationDto))
        {
            return Unauthorized();
        }

        var jwtToken = _serviceManager.AuthService.CreateTokenAsync();
        
        return Ok(jwtToken);
    }

    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<UserDetails>>> GetUsers()
    {
        var usersToReturn = await _serviceManager.AuthService.GetUsersAsync();
        
        return Ok(usersToReturn);
    }

    [HttpGet("users/{id}")]
    public async Task<ActionResult<UserDetails>> GetUserById(string id)
    {
        var userToReturn = await _serviceManager.AuthService.GetUserByIdAsync(id);
        return Ok(userToReturn);
    }
}