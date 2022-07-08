using Botec.CommandProcessor.Answers;
using Botec.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Botec.CommandProcessor.CommandsLogic;

public class FuckLogic
{
    private readonly IAccountRepository _accountRepository;

    public FuckLogic(IServiceProvider services)
    {
        _accountRepository = services.GetRequiredService<IAccountRepository>();
    }

    public async Task FuckSomebody(
        ITelegramBotClient botClient, Update update, string command, CancellationToken cancellationToken)
    {
        var chat = update.Message!.Chat!;
        var from = update.Message!.From!;
        
        var accounts = (await _accountRepository.GetAllAccountsFromChatAsync(chat.Id, cancellationToken)).ToList();

        var random = new Random();
        var randomUser = accounts[random.Next(0, accounts.Count - 1)];

        var answer = randomUser.Id == from.Id
            ? FuckAnswers.GetFuckHimselfString(from.FirstName, from.LastName)
            : FuckAnswers.GetFuckString(from.FirstName, from.LastName, randomUser.FirstName, randomUser.LastName);

        await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: answer,
            cancellationToken: cancellationToken);
    }
}