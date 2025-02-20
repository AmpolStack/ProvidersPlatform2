using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using ProvidersPlatform.EmailServices.Grpc;
using ProvidersPlatform.UserServices.Rest.Utilities;
using ProvidersPlatform.UserServices.Services.Definitions;

namespace ProvidersPlatform.UserServices.Rest.Controllers;

[ApiController]
[Route("User")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly MailOperations.MailOperationsClient _operationsClient;

    public UserController(IUserRepository userRepository, IMapper mapper, MailOperations.MailOperationsClient operationsClient)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _operationsClient = operationsClient;
    }

    [HttpPost("Test")]
    public Task<ActionResult<bool>> TestAsync([FromBody] Thing thing)
    {
        return Task.FromResult<ActionResult<bool>>(Ok("test"));
    }
    [HttpGet("Secure/GetUserById")]
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

    [HttpGet("SendMailForUserById")]
    public async Task<IActionResult> SendMailForUserByIdAsync([FromQuery] int userId,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            return NotFound("User not found");
        }

        var subject = $"Hi {user.Name}, recovered password";
        var body = $"If you want needed other considerations, please contact us at {user.Email}";


        var request = new MailRequest()
        {
            AliasTo = user.Name,
            To = "sacount571@gmail.com",
            Body = body,
            Subject = subject
        };

        try
        {
            var call = _operationsClient.SendMailAsync(request, cancellationToken: cancellationToken,
                deadline: DateTime.UtcNow.AddMinutes(1));
            var smtpResponse = await call;
            var time = call.GetTrailers().Get("ExecTime")?.Value;

            if (!smtpResponse.Success)
            {
                return BadRequest(smtpResponse);
            }

            return Ok(new
            {
                TimeInMilisecods = time,
                Data = smtpResponse
            });
        }
        catch (RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.DeadlineExceeded)
        {
            return BadRequest("Deadline exceeded");
        }
        
    }
    
}

public record Thing(string Name, string Email);
