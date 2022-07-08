using Telegram.Bot.Types;

namespace Botec.CommandProcessor.Interfaces;

public interface IUserProcessingService
{
    Task RegisterUserIfNotExistAsync(Update update, CancellationToken cancellationToken);
    Task RegisterUserInTheChatIfNotRegisteredAsync(Update update, CancellationToken cancellationToken);
}