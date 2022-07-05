using Botec.CommandProcessor.Answers;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Botec.CommandProcessor.CommandsLogic;

public class JokeLogic
{
    public static async Task PrintBaneksJoke(
        ITelegramBotClient botClient, Update update, string command, CancellationToken cancellationToken)
    {
        using var client = new HttpClient();
        var htmlDocument = new HtmlDocument();
        
        htmlDocument.LoadHtml(await client.GetStringAsync("https://baneks.ru/random", cancellationToken));

        var joke = htmlDocument.DocumentNode.QuerySelector("p").InnerText;

        await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: joke ?? JokeAnswers.GetJokeExceptionUserMessage(),
            cancellationToken: cancellationToken);
    }
}