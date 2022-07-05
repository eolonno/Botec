using Botec.Domain.Entities;
using Botec.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Botec.Domain.Repositories;

public class ChatRepository : IChatRepository
{
    private ApplicationDbContext _context;

    public ChatRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddChatAsync(Chat chat, CancellationToken cancellationToken)
    {
        await _context.Chat.AddAsync(chat, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddAccountToTheChatAsync(long accountId, long chatId, CancellationToken cancellationToken)
    {
        var account = await _context.Account.FirstOrDefaultAsync(x => x.Id == accountId, cancellationToken);
        var chat = (await _context.Chat.Where(x => x.Id == chatId).FirstOrDefaultAsync(cancellationToken))!;

        chat.Accounts ??= new List<Account>();

        chat.Accounts.Add(account!);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Chat?> GetChatById(long chatId, CancellationToken cancellationToken)
    {
        return await _context.Chat
            .AsNoTracking()
            .Where(x => x.Id == chatId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddFaggotOfTheDay(long chatId, Account faggotOfTheDay, CancellationToken cancellationToken)
    {
        var chat = await _context.Chat.Where(x => x.Id == chatId).FirstOrDefaultAsync(cancellationToken);

        chat.FaggotOfTheDayId = faggotOfTheDay.Id;
        chat.LastFaggotChangeDate = DateTime.Today;
        await _context.SaveChangesAsync(cancellationToken);
    }
}