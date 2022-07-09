using Botec.Application.Utilities;
using Botec.CommandProcessor;
using Botec.CommandProcessor.CommandsLogic;
using Botec.CommandProcessor.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Botec.Application;

public class MessageProcessingService
{
    private readonly Dictionary<IEnumerable<string>, Func<ITelegramBotClient, Update, string, CancellationToken, Task>> _commandsDictionary;

    private readonly IChatProcessingService _chatProcessingService;
    private readonly IUserProcessingService _userProcessingService;

    private readonly MessageLogger _messageLogger;
    private readonly RepeatLogic _repeatLogic;
    private readonly CockLogic _cockLogic;
    private readonly FaggotOfTheDayLogic _faggotOfTheDayLogic;
    private readonly JokeLogic _jokeLogic;
    private readonly FuckLogic _fuckLogic;
    private readonly NicknameOfTheDayLogic _nicknameOfTheDayLogic;
    private readonly RandomEmojiLogic _randomEmojiLogic;

    public MessageProcessingService(IServiceProvider services)
    {
        _messageLogger = new MessageLogger(services);
        _repeatLogic = new RepeatLogic();
        _cockLogic = new CockLogic(services);
        _faggotOfTheDayLogic = new FaggotOfTheDayLogic(services);
        _jokeLogic = new JokeLogic();
        _fuckLogic = new FuckLogic(services);
        _nicknameOfTheDayLogic = new NicknameOfTheDayLogic(services);
        _randomEmojiLogic = new RandomEmojiLogic();

        _chatProcessingService = services.GetRequiredService<IChatProcessingService>();
        _userProcessingService = services.GetRequiredService<IUserProcessingService>();

        _commandsDictionary = GetCommandsDictionary();
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

        foreach (var command in _commandsDictionary)
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
            { Commands.CocksTopCommands, _cockLogic.PrintCocksTopAsync },
            { Commands.AbsoluteRatingCommands, _cockLogic.PrintAbsoluteCockPositionAsync },
            { Commands.FaggotOfTheDayCommands, _faggotOfTheDayLogic.PrintFaggotOfTheDayAsync },
            { Commands.BanekJokeCommands, _jokeLogic.PrintBaneksJokeAsync },
            { Commands.FuckCommands, _fuckLogic.FuckSomebodyAsync },
            { Commands.NickNameOfTheDayCommands, _nicknameOfTheDayLogic.PrintNicknameOfTheDayAsync },
            { Commands.RandomEmojiCommands, _randomEmojiLogic.PrintRandomEmojiAsync }

        };

        return commands;
    }
}