using Botec.Domain.Entities;
using Botec.Domain.Enums;
using Botec.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace Botec.Application.Utilities;

public class MessageLogger
{
    private readonly IMessageLogRepository _messageLogRepository;

    public MessageLogger(IServiceProvider services)
    {
        _messageLogRepository = services.GetRequiredService<IMessageLogRepository>();
    }

    public async Task LogUpdate(Update update, CancellationToken cancellationToken)
    {
        var from = update.Message?.From;
        var message = update.Message;

        var messageLog = new MessageLog
        {
            Id = Guid.NewGuid(),
            AccountId = from?.Id,
            Username = from?.Username,
            FirstName = from?.FirstName,
            LastName = from?.LastName,
            ChatId = message?.Chat.Id,
            ChatTitle = message?.Chat.Title,
            Text = message?.Text,
            MessengerType = MessengerType.Telegram,
            Date = message?.Date ?? DateTime.Now,
            JsonUpdate = JsonConvert.SerializeObject(update)
        };

        await _messageLogRepository.LogUpdate(messageLog, cancellationToken);
    }
}