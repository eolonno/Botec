using Botec.Domain.Entities;

namespace Botec.Domain.Interfaces;

public interface IChatRepository
{
    Task AddChatAsync(Chat chat, CancellationToken cancellationToken);
    Task AddAccountToTheChatAsync(long accountId, long chatId, CancellationToken cancellationToken);
    Task<Chat?> GetChatById(long chatId, CancellationToken cancellationToken);
    Task AddFaggotOfTheDay(long chatId, Account faggotOfTheDay, CancellationToken cancellationToken);
}