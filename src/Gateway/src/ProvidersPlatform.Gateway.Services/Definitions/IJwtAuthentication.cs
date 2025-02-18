using ProvidersPlatform.Gateway.Services.Custom;

namespace ProvidersPlatform.Gateway.Services.Definitions;

public interface IJwtAuthentication
{
    public Task<AuthenticationResponse>
        Authenticate(AuthenticationRequest request, CancellationToken cancellationToken);
    
    public Task<AuthenticationResponse>
        Authenticate(Credentials credentials, CancellationToken cancellationToken);
}