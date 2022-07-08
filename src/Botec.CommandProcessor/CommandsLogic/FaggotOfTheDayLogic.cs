using Botec.CommandProcessor.Answers;
using Botec.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Botec.CommandProcessor.CommandsLogic;

public class FaggotOfTheDayLogic
{
    private readonly IAccountRepository _accountRepository;
    private readonly IChatRepository _chatRepository;

    public FaggotOfTheDayLogic(IServiceProvider services)
    {
        _accountRepository = services.GetRequiredService<IAccountRepository>();
        _chatRepository = services.GetRequiredService<IChatRepository>();
    }

    public async Task PrintFaggotOfTheDay(
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