using Botec.Domain.Entities;

namespace Botec.Domain.Interfaces;

public interface IMessageLogRepository
{
    Task LogUpdate(MessageLog messageLog, CancellationToken cancellationToken);
}