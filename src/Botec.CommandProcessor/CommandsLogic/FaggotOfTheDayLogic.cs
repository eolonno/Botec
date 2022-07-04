using Botec.CommandProcessor.Answers;
using Botec.Domain.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Botec.CommandProcessor.CommandsLogic;

public class FaggotOfTheDayLogic
{
    private static AccountRepository _accountRepository = new();
    private static ChatRepository _chatRepository = new();

    public static async Task PrintFaggotOfTheDay(
        ITelegramBotClient botClient, Update update, string command, CancellationToken cancellationToken)
    {
        var chat = update.Message!.Chat!;

        var currentFaggotOfTheDay = await _accountRepository.GetFaggotOfTheDay(chat.Id, cancellationToken);

        if (currentFaggotOfTheDay is null)
        {
            var accounts = (await _accountRepository.GetAllAccountsFromChatAsync(chat.Id, cancellationToken)).ToList();

            var random = new Random();
            var faggotOfTheDay = accounts[random.Next(0, accounts.Count - 1)];

            await _chatRepository.AddFaggotOfTheDay(chat.Id, faggotOfTheDay, cancellationToken);
        }

        currentFaggotOfTheDay = await _accountRepository.GetFaggotOfTheDay(chat.Id, cancellationToken);

        await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: FaggotOfTheDayAnswers.GetFaggotOfTheDayString(currentFaggotOfTheDay!.FirstName, currentFaggotOfTheDay!.LastName),
            cancellationToken: cancellationToken);
    }
}