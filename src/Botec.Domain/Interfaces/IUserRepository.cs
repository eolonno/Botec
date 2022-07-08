using Botec.Domain.Entities;

namespace Botec.Domain.Interfaces;

public interface IUserRepository
{
    Task AddUserAsync(User user, CancellationToken cancellationToken);
    Task<User?> GetUserByAccountAsync(Account account, CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetAllUsers(CancellationToken cancellationToken);
}