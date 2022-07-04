using Botec.Domain.Entities;

namespace Botec.Domain.Repositories;

public class MessageLogRepository
{
    private ApplicationDbContext _context;

    public MessageLogRepository()
    {
        _context = new ApplicationDbContext();
    }

    public async Task LogUpdate(MessageLog messageLog, CancellationToken cancellationToken)
    {
        await _context.AddAsync(messageLog, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}