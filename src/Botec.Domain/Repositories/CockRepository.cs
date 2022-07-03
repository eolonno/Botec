﻿using Botec.Domain.Entities;
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

    public async Task<DateTime?> GetLastCommitDateAsync(long accountId, CancellationToken cancellationToken)
    {
        return await _context.Account
            .Where(x => x.Id == accountId)
            .Include(x => x.User)
            .Include(x => x.User.Cock)
            .Select(x => x.User)
            .Select(x => x.Cock.LastCommitDate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> GetIronCockStatusAsync(long accountId, CancellationToken cancellationToken)
    {
        return await _context.Account
            .Include(x => x.User)
            .Include(x => x.User.Cock)
            .Where(x => x.Id == accountId)
            .Select(x => x.User.HasIronCock)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> GetCockLength(long accountId, CancellationToken cancellationToken)
    {
        return await _context.Account
            .Include(x => x.User)
            .Include(x => x.User.Cock)
            .Where(x => x.Id == accountId)
            .Select(x => x.User.Cock.Length)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateCockAsync(long accountId, int lengthToChange, CancellationToken cancellationToken)
    {
        var cock = await _context.Account
            .Include(x => x.User)
            .Include(x => x.User.Cock)
            .Where(x => x.Id == accountId)
            .Select(x => x.User.Cock)
            .FirstOrDefaultAsync(cancellationToken);

        cock.Length += lengthToChange;
        cock.LastCommitDate = DateTime.Today;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DoubleCockAsync(long accountId, CancellationToken cancellationToken)
    {
        var cock = await _context.Account
            .Include(x => x.User)
            .Include(x => x.User.Cock)
            .Where(x => x.Id == accountId)
            .Select(x => x.User.Cock)
            .FirstOrDefaultAsync(cancellationToken);

        cock.Length *= 2;
        cock.LastCommitDate = DateTime.Today;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task CircumciseCockAsync(long accountId, CancellationToken cancellationToken)
    {
        var cock = await _context.Account
            .Include(x => x.User)
            .Include(x => x.User.Cock)
            .Where(x => x.Id == accountId)
            .Select(x => x.User.Cock)
            .FirstOrDefaultAsync(cancellationToken);

        cock.Length = 0;
        cock.LastCommitDate = DateTime.Today;

        await _context.SaveChangesAsync(cancellationToken);
    }
}