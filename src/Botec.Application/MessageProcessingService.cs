using Botec.Application.Utilities;
using Botec.CommandProcessor;
using Botec.CommandProcessor.CommandsLogic;
using Botec.CommandProcessor.Interfaces;
using Botec.CommandProcessor.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Botec.Application;

public class MessageProcessingService
{
    private readonly Dictionary<IEnumerable<string>, Func<ITelegramBotClient, Update, string, CancellationToken, Task>> CommandsDictionary;

    private readonly IChatProcessingService _chatProcessingService;
    private readonly IUserProcessingService _userProcessingService;

    private readonly MessageLogger _messageLogger;
    private readonly RepeatLogic _repeatLogic;
    private readonly CockLogic _cockLogic;
    private readonly FaggotOfTheDayLogic _faggotOfTheDayLogic;
    private readonly JokeLogic _jokeLogic;


    public MessageProcessingService(IServiceProvider services)
    {
        _messageLogger = new MessageLogger(services);
        _repeatLogic = new RepeatLogic();
        _cockLogic = new CockLogic(services);
        _faggotOfTheDayLogic = new FaggotOfTheDayLogic(services);
        _jokeLogic = new JokeLogic();

        _chatProcessingService = services.GetRequiredService<IChatProcessingService>();
        _userProcessingService = services.GetRequiredService<IUserProcessingService>();

        CommandsDictionary = GetCommandsDictionary();
    }

    public async Task ProcessMessage(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        await _messageLogger.LogUpdate(update, cancellationToken);

        var message = update.Message?.Text;

        if (message is null)
            return;

        JsonSerializer.Create().Serialize(Console.Out, update);

        await _chatProcessingService.RegisterChatIfNotRegistered(update, cancellationToken);
        await _userProcessingService.RegisterUserIfNotExistAsync(update, cancellationToken);
        await _userProcessingService.RegisterUserInTheChatIfNotRegisteredAsync(update, cancellationToken);

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

    private Dictionary<IEnumerable<string>, Func<ITelegramBotClient, Update, string, CancellationToken, Task>> GetCommandsDictionary()
    {
        var commands = new Dictionary<IEnumerable<string>, Func<ITelegramBotClient, Update, string, CancellationToken, Task>>
        {
            { Commands.RepeatCommands, _repeatLogic.RepeatAsync },
            { Commands.UpdateCockCommands, _cockLogic.UpdateCockAsync },
            { Commands.CocksTopCommands, _cockLogic.PrintCocksTop },
            { Commands.AbsoluteRatingCommands, _cockLogic.PrintAbsoluteCockPosition },
            { Commands.FaggotOfTheDayCommands, _faggotOfTheDayLogic.PrintFaggotOfTheDay },
            { Commands.BanekJokeCommands, _jokeLogic.PrintBaneksJoke },
        };

        return commands;
    }
}