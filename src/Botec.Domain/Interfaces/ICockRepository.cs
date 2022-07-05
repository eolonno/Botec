namespace Botec.Domain.Interfaces;

public interface ICockRepository
{
    Task<DateTime?> GetLastCommitDateAsync(long accountId, CancellationToken cancellationToken);
    Task<bool> GetIronCockStatusAsync(long accountId, CancellationToken cancellationToken);
    Task<int> GetCockLength(long accountId, CancellationToken cancellationToken);
    Task UpdateCockAsync(long accountId, int lengthToChange, CancellationToken cancellationToken);
    Task DoubleCockAsync(long accountId, CancellationToken cancellationToken);
    Task CircumciseCockAsync(long accountId, CancellationToken cancellationToken);
}