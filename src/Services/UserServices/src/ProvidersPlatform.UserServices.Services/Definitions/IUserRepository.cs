using ProvidersPlatform.Shared.Models;
using ProvidersPlatform.Shared.Setup.Defaults;

namespace ProvidersPlatform.UserServices.Services.Definitions;

public interface IUserRepository 
{
    public Task<User?> GetUserByIdAsync(int userId, CancellationToken cancellationToken);
    public Task<User?> GetUserAndProviderByIdAsync(int userId, CancellationToken cancellationToken);
    public Task<User?> GetUserByCredentialsAsync(string email, string password, CancellationToken cancellationToken);
    public Task<bool> DeleteUserByIdAsync(int id, CancellationToken cancellationToken);
    public Task<bool> UpdatePasswordAsync(int id, string password, CancellationToken cancellationToken);
}