using Microsoft.EntityFrameworkCore;
using TargetAPI.Data;
using TargetAPI.Exceptions;
using TargetAPI.Repositories.Abstract;

namespace TargetAPI.Repositories.Concrete;

public class IdentityRepository : IIdentityRepository
{
    private readonly TargetDbContext _dbContext;

    private readonly DbSet<User> _table;

    public IdentityRepository(TargetDbContext dbContext)
    {
        _dbContext = dbContext;
        _table = _dbContext.Set<User>();
    }

    public async Task<User> GetUserByLogin(string login)
    {
        return await _table.FindAsync(login) ?? throw new EntityNotFoundException("Login", login);
    }

    public async Task<bool> AddUser(User user)
    {
        var existingUser = await _table.FindAsync(user.Login);
        if (existingUser != null)
            return false;
            
        await _table.AddAsync(user);
        var entriesWritten = await _dbContext.SaveChangesAsync();
        return entriesWritten > 0;
    }
}