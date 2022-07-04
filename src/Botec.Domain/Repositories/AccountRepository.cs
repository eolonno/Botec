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
        var account = await _context.Account.FirstOrDefaultAsync(x => x.Id == accountId, cancellationToken);
        return account;
    }

    public async Task CreateAccountAsync(Account account, CancellationToken cancellationToken)
    {
        await _context.AddAsync(account, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Account>> GetAllAccountsAsync(CancellationToken cancellationToken)
    {
        return await _context.Account
            .Include(x => x.User)
            .ThenInclude(x => x.Cock)
            .Include(x => x.User)
            .ThenInclude(x => x.NicknameOfTheDay)
            .ThenInclude(x => x.Nickname)
            .ToListAsync(cancellationToken);
    }
}