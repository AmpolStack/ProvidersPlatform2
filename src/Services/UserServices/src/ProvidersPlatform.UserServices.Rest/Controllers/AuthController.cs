using Microsoft.AspNetCore.Mvc;
using ProvidersPlatform.UserServices.Services.Custom;
using ProvidersPlatform.UserServices.Services.Definitions;
using ProvidersPlatform.UserServices.Services.Models;

namespace ProvidersPlatform.UserServices.Rest.Controllers;

[ApiController]
[Route("Auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtAuthentication _jwt;

    public AuthController(IUserRepository userRepository, IJwtAuthentication jwt)
    {
        _userRepository = userRepository;
        _jwt = jwt;
    }

    [HttpPost("WithRefreshToken")]
    public async Task<IActionResult> WithRefreshToken([FromBody] AuthenticationRequest request, [FromQuery] int id, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(request.RefreshToken) || string.IsNullOrEmpty(request.Token))
        {
            return BadRequest("The tokens are required");
        }

        var areExpired = await _jwt.SessionAlreadyExist(request, id, ct);

        if (!areExpired)
        {
            return Unauthorized("The session are expired, login again");
        }
        
        var userFind = await _userRepository.GetUserByIdAsync(id, ct);

        if (userFind == null)
        {
            return NotFound("User not found");
        }

        var response = await _jwt.Authenticate(userFind, ct);
        
        return Ok(response);
    }
    
    
    [HttpPost("WithCredentials")]
    public async Task<IActionResult> WithCredentials([FromBody] Credentials credentials, CancellationToken ct)
    {
        if (credentials.Email.Equals(string.Empty) || credentials.Password.Equals(string.Empty))
        {
            return BadRequest("Email or Password are empty, try again");
        }
        
        var userFind = await _userRepository.GetUserByCredentialsAsync(credentials.Email, credentials.Password, ct);

        if (userFind == null)
        {
            return NotFound("There is no user with those credentials");
        }
        
        var response = await _jwt.Authenticate(userFind, ct);
        
        return Ok(response);
    }
}