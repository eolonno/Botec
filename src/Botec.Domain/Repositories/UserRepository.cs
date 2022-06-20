using Botec.Domain.Entities;

namespace Botec.Domain.Repositories;

public class UserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository()
    {
        _context = new ApplicationDbContext();
    }

    public async Task AddUser(User user)
    {
        await _context.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}