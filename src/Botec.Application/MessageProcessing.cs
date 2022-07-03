using Botec.CommandProcessor;
using Botec.CommandProcessor.CommandsLogic;
using Botec.CommandProcessor.Services;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Botec.Application;

public class MessageProcessing
{
    private static readonly Dictionary<IEnumerable<string>, Func<ITelegramBotClient, Update, string, CancellationToken, Task>> CommandsDictionary;

    static MessageProcessing()
    {
        CommandsDictionary = GetCommandsDictionary();
    }

    public static async Task ProcessMessage(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        JsonSerializer.Create().Serialize(Console.Out, update);
        var message = update.Message?.Text;

        if (message is null)
            return;

        await UserProcessingService.RegisterUserIfNotExistAsync(update, cancellationToken);

        foreach (var command in CommandsDictionary)
        {
            var sessionCommand = command.Key.FirstOrDefault(x => message.ToLower().StartsWith(x));
            if (sessionCommand is not null)
            {
                await command.Value(botClient, update, sessionCommand, cancellationToken);
                return;
            }
        }
    }

    private static Dictionary<IEnumerable<string>, Func<ITelegramBotClient, Update, string, CancellationToken, Task>> GetCommandsDictionary()
    {
        var commands = new Dictionary<IEnumerable<string>, Func<ITelegramBotClient, Update, string, CancellationToken, Task>>
        {
            { Commands.RepeatCommands, RepeatLogic.RepeatAsync },
            { Commands.UpdateCockCommands, CockLogic.UpdateCockAsync },
            { Commands.CocksTopCommands, CockLogic.PrintCocksTop },
        };

        return commands;
    }
}