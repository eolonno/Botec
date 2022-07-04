using Botec.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Botec.Domain.Repositories;

public class UserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository()
    {
        _context = new ApplicationDbContext();
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
            .Include(x => x.Chats)
            .Include(x => x.NicknameOfTheDay)
            .ThenInclude(x => x.Nickname)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllUsersFromChat(long chatId, CancellationToken cancellationToken)
    {
        var chat = await _context.Chat
            .AsNoTracking()
            .Where(x => x.Id == chatId)
            .Include(x => x.Users)
            .ThenInclude(x => x.Cock)
            .Include(x => x.Users)
            .ThenInclude(x => x.Accounts)
            .FirstOrDefaultAsync(cancellationToken);

        return chat!.Users;
    }
}