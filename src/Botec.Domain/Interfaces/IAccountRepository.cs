using Botec.Domain.Entities;

namespace Botec.Domain.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetAccountByAccountIdAsync(long accountId, CancellationToken cancellationToken);
    Task CreateAccountAsync(Account account, CancellationToken cancellationToken);
    Task<IEnumerable<Account>> GetAllAccountsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Account>> GetAllAccountsFromChatAsync(long chatId, CancellationToken cancellationToken);
    Task<Account?> GetFaggotOfTheDay(long chatId, CancellationToken cancellationToken);
}