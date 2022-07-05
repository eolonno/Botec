using Botec.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Botec.Domain.Repositories;

public class AccountRepository
{
    private readonly ApplicationDbContext _context;

    public AccountRepository()
    {
        _context = new ApplicationDbContext();
    }

    public async Task<Account?> GetAccountByAccountIdAsync(long accountId, CancellationToken cancellationToken)
    {
        return await _context.Account
            .AsNoTracking()
            .Include(x => x.Chats)
            .FirstOrDefaultAsync(x => x.Id == accountId, cancellationToken);
    }

    public async Task CreateAccountAsync(Account account, CancellationToken cancellationToken)
    {
        await _context.AddAsync(account, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Account>> GetAllAccountsAsync(CancellationToken cancellationToken)
    {
        return await _context.Account
            .AsNoTracking()
            .Include(x => x.User)
            .ThenInclude(x => x.Cock)
            .Include(x => x.User)
            .ThenInclude(x => x.NicknameOfTheDay)
            .ThenInclude(x => x.Nickname)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Account>> GetAllAccountsFromChatAsync(long chatId, CancellationToken cancellationToken)
    {
        var chat = await _context.Chat
            .AsNoTracking()
            .Where(x => x.Id == chatId)
            .Include(x => x.Accounts)
            .ThenInclude(x => x.User)
            .ThenInclude(x => x.Cock)
            .FirstOrDefaultAsync(cancellationToken);

        return chat!.Accounts;
    }

    public async Task<Account?> GetFaggotOfTheDay(long chatId, CancellationToken cancellationToken)
    {
        return await _context.Chat
            .AsNoTracking()
            .Include(x => x.FaggotOfTheDay)
            .Where(x => x.Id == chatId)
            .Where(x => x.LastFaggotChangeDate == DateTime.Today)
            .Select(x => x.FaggotOfTheDay)
            .FirstOrDefaultAsync(cancellationToken);
    }
}