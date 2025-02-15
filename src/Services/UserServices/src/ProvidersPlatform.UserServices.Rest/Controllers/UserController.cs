using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProvidersPlatform.UserServices.Rest.Utilities;
using ProvidersPlatform.UserServices.Services.Definitions;

namespace ProvidersPlatform.UserServices.Rest.Controllers;

[ApiController]
[Route("User")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpGet("GetUserById")]
    public async Task<IActionResult> GetUserByIdAsync([FromQuery] int userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            return NotFound("User not found");
        }
        var mapUser = _mapper.Map<UserDto>(user);
        return Ok(new
        {
            reponse = mapUser,
            code = 200
        });
    }
    
    [HttpGet("GetUserAndProviderById")]
    public async Task<IActionResult> GetUserAndProviderByIdAsync([FromQuery] int userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserAndProviderByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            return NotFound("User not found");
        }
        var mapUser = _mapper.Map<UserDto>(user);
        return Ok(new
        {
            reponse = mapUser,
            code = 200
        });
    }
    
}