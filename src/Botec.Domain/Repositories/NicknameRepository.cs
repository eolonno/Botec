using Botec.Domain.Entities;
using Botec.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Botec.Domain.Repositories;

public class NicknameRepository : INicknameRepository
{
    private readonly ApplicationDbContext _context;

    public NicknameRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Nickname>> GetAllNicknames(CancellationToken cancellationToken)
    {
        return await _context.Nickname
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<NicknameIntroductoryPhrase>> GetAllNicknameIntroductoryPhrases(
        CancellationToken cancellationToken)
    {
        return await _context.NicknameIntroductoryPhrase
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateNicknameOfTheDay(long accountId, Guid newNicknameId, CancellationToken cancellationToken)
    {
        var user = await _context.Account
            .Where(x => x.Id == accountId)
            .Include(x => x.User)
            .ThenInclude(x => x.NicknameOfTheDay)
            .Select(x => x.User)
            .FirstOrDefaultAsync(cancellationToken);

        _context.Entry(user);

        if (user.NicknameOfTheDay is null)
        {
            user.NicknameOfTheDay = new NicknameOfTheDay { Id = Guid.NewGuid() };
            await _context.AddAsync(user.NicknameOfTheDay, cancellationToken);
        }

        user.NicknameOfTheDay.NicknameId = newNicknameId;
        user.NicknameOfTheDay.Day = DateTime.Today;
        
        await _context.SaveChangesAsync(cancellationToken);
    }
}