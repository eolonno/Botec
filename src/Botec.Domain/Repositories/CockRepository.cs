using Botec.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Botec.Domain.Repositories;

public class CockRepository
{
    private readonly ApplicationDbContext _context;

    public CockRepository()
    {
        _context = new ApplicationDbContext();
    }

    public async Task<DateTime> GetLastCommitDateAsync(long accountId, CancellationToken cancellationToken)
    {
        return await _context.Account
            .Where(x => x.AccountId == accountId)
            .Include(x => x.User)
            .Include(x => x.User.Cock)
            .Select(x => x.User)
            .Select(x => x.Cock.LastCommitDate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> GetIronCockStatusAsync(long accountId, CancellationToken cancellationToken)
    {
        var userId = await _context.Account
            .Include(x => x.User)
            .Include(x => x.User.Cock)
            .Where(x => x.AccountId == accountId)
            .Select(x => x.User.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return await _context.UserSettings
            .Include(x => x.User)
            .Where(x => x.User.Id == userId)
            .Select(x => x.HasIronCock)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateCockAsync(long accountId, int lengthToChange, CancellationToken cancellationToken)
    {
        var cock = await _context.Account
            .Include(x => x.User)
            .Include(x => x.User.Cock)
            .Where(x => x.AccountId == accountId)
            .Select(x => x.User.Cock)
            .FirstOrDefaultAsync(cancellationToken);

        _context.Attach(cock);
        cock.Length += lengthToChange;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> GetCockLength(long accountId, CancellationToken cancellationToken)
    {
        return await _context.Account
            .Include(x => x.User)
            .Include(x => x.User.Cock)
            .Select(x => x.User.Cock.Length)
            .FirstOrDefaultAsync(cancellationToken);
    }
}