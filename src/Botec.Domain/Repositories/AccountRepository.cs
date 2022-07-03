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

    public async Task<Account?> GetAccountByAccountId(long accountId, CancellationToken cancellationToken)
    {
        var account = await _context.Account.FirstOrDefaultAsync(x => x.AccountId == accountId, cancellationToken);
        return account;
    }

    public async Task CreateAccountAsync(Account account, CancellationToken cancellationToken)
    {
        await _context.AddAsync(account, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}