using TargetAPI.Data;

namespace TargetAPI.Repositories.Abstract;

public interface IIdentityRepository
{
    Task<User> GetUserByLogin(string login);

    Task<bool> AddUser(User user);
}