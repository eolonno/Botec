using Botec.CommandProcessor.Interfaces;
using Botec.Domain.Entities;
using Botec.Domain.Enums;
using Botec.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Chat = Botec.Domain.Entities.Chat;

namespace Botec.CommandProcessor.Services;

public class ChatProcessingService : IChatProcessingService
{
    private IChatRepository _chatRepository;

    public ChatProcessingService(IServiceProvider services)
    {
        _chatRepository = services.GetRequiredService<IChatRepository>();
    }

    public async Task RegisterChatIfNotRegistered(Update update, CancellationToken cancellationToken)
    {
        var workingChat = update.Message!.Chat;

        var dbChat = await _chatRepository.GetChatById(workingChat.Id, cancellationToken);

        if (dbChat is null)
        {
            var newDbChat = new Chat
            {
                Id = workingChat.Id,
                MessengerType = MessengerType.Telegram,
                Accounts = new List<Account>()
            };

            await _chatRepository.AddChatAsync(newDbChat, cancellationToken);
        }
    }
}