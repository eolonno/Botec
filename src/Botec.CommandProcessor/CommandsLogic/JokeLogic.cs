using Botec.CommandProcessor.Answers;
using Botec.CommandProcessor.Utilities.Extensions;
using Botec.Domain.Interfaces;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Botec.CommandProcessor.CommandsLogic;

public class JokeLogic
{
    private readonly IJokeRepository _jokeRepository;

    public JokeLogic(IServiceProvider services)
    {
        _jokeRepository = services.GetRequiredService<IJokeRepository>();
    }

    public async Task PrintBaneksJokeAsync(
        ITelegramBotClient botClient, Update update, string command, CancellationToken cancellationToken)
    {
        using var client = new HttpClient();
        var htmlDocument = new HtmlDocument();
        
        htmlDocument.LoadHtml(await client.GetStringAsync("https://baneks.ru/random", cancellationToken));

        var joke = htmlDocument.DocumentNode.QuerySelector("p").InnerText;

        await botClient.SendTextMessageAsync(
            chatId: update.Message!.Chat.Id,
            text: joke ?? JokeAnswers.GetJokeExceptionUserMessage(),
            cancellationToken: cancellationToken);
    }

    public async Task PrintDanilaJokeAsync(
        ITelegramBotClient botClient, Update update, string command, CancellationToken cancellationToken)
    {
        var randomJoke = (await _jokeRepository.GetAllJokesAsync(cancellationToken)).GetRandomElement();

        await botClient.SendTextMessageAsync(
            chatId: update.Message!.Chat.Id,
            text: randomJoke.Text,
            cancellationToken: cancellationToken);
    }
}