using Botec.Domain.Entities;
using Botec.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Botec.Domain.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddUserAsync(User user, CancellationToken cancellationToken)
    {
        await _context.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<User?> GetUserByAccountAsync(Account account, CancellationToken cancellationToken)
    {
        return await _context.Account
            .AsNoTracking()
            .Where(x => x.Id == account.Id)
            .Select(x => x.User)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllUsers(CancellationToken cancellationToken)
    {
        return await _context.User
            .AsNoTracking()
            .Include(x => x.Cock)
            .Include(x => x.Accounts)
            .ThenInclude(x => x.Chats)
            .Include(x => x.NicknameOfTheDay)
            .ThenInclude(x => x.Nickname)
            .ToListAsync(cancellationToken);
    }
}