using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Botec.CommandProcessor.CommandsLogic;

public class RandomEmojiLogic
{
    public async Task PrintRandomEmojiAsync(
        ITelegramBotClient botClient, Update update, string command, CancellationToken cancellationToken)
    {
        var random = new Random();
        var randomEmoji = (Emoji)random.Next(1, 6);

        await botClient.SendDiceAsync(
            chatId: update.Message!.Chat.Id,
            emoji: randomEmoji,
            cancellationToken: cancellationToken);
    }
}