using Botec.Domain.Entities;

namespace Botec.Domain.Interfaces;

public interface IJokeRepository
{
    Task<IEnumerable<Joke>> GetAllJokesAsync(CancellationToken cancellationToken);
    Task CreateJoke(string text, CancellationToken cancellationToken);
}