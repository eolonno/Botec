using Botec.Domain.Entities;
using Botec.Domain.Interfaces;

namespace Botec.Domain.Repositories;

public class MessageLogRepository : IMessageLogRepository
{
    private ApplicationDbContext _context;

    public MessageLogRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task LogUpdate(MessageLog messageLog, CancellationToken cancellationToken)
    {
        await _context.AddAsync(messageLog, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}