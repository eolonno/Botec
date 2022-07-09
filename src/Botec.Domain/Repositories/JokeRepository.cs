using Botec.Domain.Entities;
using Botec.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Botec.Domain.Repositories;

public class JokeRepository : IJokeRepository
{
    private readonly ApplicationDbContext _context;

    public JokeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Joke>> GetAllJokesAsync(CancellationToken cancellationToken)
    {
        return await _context.Joke
            .ToListAsync(cancellationToken);
    }

    public async Task CreateJoke(string text, CancellationToken cancellationToken)
    {
        var joke = new Joke
        {
            Id = Guid.NewGuid(),
            Text = text,
        };

        await _context.AddAsync(joke, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}