using Botec.CommandProcessor;
using Botec.CommandProcessor.CommandsLogic;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Botec.Application;

public class MessageProcessing
{
    private static readonly Dictionary<IEnumerable<string>, Func<ITelegramBotClient, Update, CancellationToken, string, Task>> CommandsDictionary;

    static MessageProcessing()
    {
        CommandsDictionary = GetCommandsDictionary();
    }

    public static async Task ProcessMessage(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var message = update.Message?.Text;

        if (message is null)
            return;

        foreach (var command in CommandsDictionary)
        {
            var sessionCommand = command.Key.FirstOrDefault(x => message.StartsWith(x));
            if (sessionCommand is not null)
            {
                await command.Value(botClient, update, cancellationToken, sessionCommand);
                return;
            }
        }
    }

    private static Dictionary<IEnumerable<string>, Func<ITelegramBotClient, Update, CancellationToken, string, Task>> GetCommandsDictionary()
    {
        var commands = new Dictionary<IEnumerable<string>, Func<ITelegramBotClient, Update, CancellationToken, string, Task>>
        {
            { Commands.RepeatCommands, RepeatLogic.RepeatAsync }
        };

        return commands;
    }
}