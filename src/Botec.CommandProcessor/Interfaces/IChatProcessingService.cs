using Telegram.Bot.Types;

namespace Botec.CommandProcessor.Interfaces;

public interface IChatProcessingService
{
    Task RegisterChatIfNotRegistered(Update update, CancellationToken cancellationToken);
}