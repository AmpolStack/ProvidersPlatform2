using Microsoft.EntityFrameworkCore;
using ProvidersPlatform.Shared.Models;
using ProvidersPlatform.Shared.Setup.Defaults;
using ProvidersPlatform.UserServices.Services.Definitions;

namespace ProvidersPlatform.UserServices.Services.Implementations;

public class UserRepository : IUserRepository
{
    private readonly IGenericRepository<User> _repository;

    public UserRepository(IGenericRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task<User?> GetUserByIdAsync(int userId, CancellationToken cancellationToken)
    {
        var response = await _repository.GetEntity(x => x.UserId == userId, cancellationToken);
        return response;
    }

    public async Task<User?> GetUserAndProviderByIdAsync(int userId, CancellationToken cancellationToken)
    {
        IQueryable<User> response = await _repository.GetEntities(x => x.UserId == userId);
        var lam = await response.Include(x => x.Provider).FirstAsync(cancellationToken);
        return lam;
    }

    public async Task<User?> GetUserByCredentialsAsync(string email, string password, CancellationToken cancellationToken)
    {
        var response = await _repository.GetEntity(x => x.Password == password && x.Email == email, cancellationToken);
        return response;
    }

    public async Task<bool> DeleteUserByIdAsync(int id, CancellationToken cancellationToken)
    {
        var findUser = await _repository.GetEntity(x => x.UserId == id, cancellationToken);
        if (findUser == null)
        {
            return false;
        }
        var response = await _repository.Delete(findUser, cancellationToken);
        return response;
    }

    public async Task<bool> UpdatePasswordAsync(int id, string password, CancellationToken cancellationToken)
    {
        var findUser = await _repository.GetEntity(x => x.UserId == id, cancellationToken);
        if (findUser == null)
        {
            return false;
        }
        findUser.Password = password;
        return await _repository.Update(findUser, cancellationToken);
    }
}