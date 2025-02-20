using ProvidersPlatform.Shared.Models;
using ProvidersPlatform.UserServices.Services.Custom;

namespace ProvidersPlatform.UserServices.Services.Definitions;

public interface IJwtAuthentication
{
    public Task<bool>
        SessionAlreadyExist(AuthenticationRequest request, int id,  CancellationToken cancellationToken);
    
    public Task<AuthenticationResponse>
        Authenticate(User user, CancellationToken cancellationToken);
}