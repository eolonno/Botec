using Botec.CommandProcessor;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Botec.Application;

public class MessageProcessing
{
    private static readonly Dictionary<string, Func<ITelegramBotClient, Update, string, CancellationToken, Task>> Commands;

    static MessageProcessing()
    {
        Commands = GetCommandsDictionaryAsync();
    }

    public static async Task ProcessMessage(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var message = update.Message?.Text;

        if (message is null)
            return;

        foreach (var command in Commands)
        {
            if (message.StartsWith(command.Key))
            {
                await command.Value(botClient, update, command.Key, cancellationToken);
                return;
            }
        }
    }

    private static Dictionary<string, Func<ITelegramBotClient, Update, string, CancellationToken, Task>> GetCommandsDictionaryAsync()
    {
        var commands = new Dictionary<string, Func<ITelegramBotClient, Update, string, CancellationToken, Task>>();
        commands.Add("бот повтори", RepeatLogic.RepeatAsync);

        return commands;
    }
}