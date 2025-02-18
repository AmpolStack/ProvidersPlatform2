using ProvidersPlatform.Gateway.Data.Models;
using ProvidersPlatform.Gateway.Services.Custom;
using ProvidersPlatform.Gateway.Services.Definitions;

namespace ProvidersPlatform.Gateway.Services.Implementations;


//TODO : Implements the logic of authenticate, and use in gatewayApi
public class JwtAuthentication : IJwtAuthentication
{
    private readonly JwtDbContext _context;
    public Task<AuthenticationResponse> Authenticate(AuthenticationRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<AuthenticationResponse> Authenticate(Credentials credentials, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}