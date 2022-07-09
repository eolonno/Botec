using Botec.CommandProcessor.Answers;
using Botec.CommandProcessor.Enums;
using Botec.CommandProcessor.Utilities;
using Botec.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Botec.CommandProcessor.CommandsLogic;

// TODO: uncomment code
public class CockLogic
{
    private readonly ICockRepository _cockRepository;
    private readonly IAccountRepository _accountRepository;

    public CockLogic(IServiceProvider services)
    {
        _cockRepository = services.GetRequiredService<ICockRepository>();
        _accountRepository = services.GetRequiredService<IAccountRepository>();
    }

    public async Task UpdateCockAsync(
        ITelegramBotClient botClient, Update update, string command, CancellationToken cancellationToken)
    {
        var from = update.Message!.From!;
        var lastCommitDate = await _cockRepository.GetLastCommitDateAsync(from.Id, cancellationToken);

        //if (lastCommitDate.Date != DateTime.Today)
        //{
        var hasIronCock = await _cockRepository.GetIronCockStatusAsync(from.Id, cancellationToken);

        var randomNumber = RandomWithProbability.GetRandomNumber();

        while (hasIronCock && randomNumber == (int)CockConstants.Circumcision)
        {
            randomNumber = RandomWithProbability.GetRandomNumber();
        }

        switch (randomNumber)
        {
            case (int)CockConstants.Circumcision:
                await _cockRepository.CircumciseCockAsync(from.Id, cancellationToken);
                break;
            case (int)CockConstants.Double:
                await _cockRepository.DoubleCockAsync(from.Id, cancellationToken);
                break;
            default:
                await _cockRepository.UpdateCockAsync(from.Id, randomNumber, cancellationToken);
                break;
        }

        var resultLength = await _cockRepository.GetCockLength(from.Id, cancellationToken);

        var answer = CockAnswers.GetCockUpdateAnswer(from.FirstName, from.LastName, randomNumber);
        var resultLengthAnswer = CockAnswers.GetResultCockLength(resultLength);

        await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: answer + resultLengthAnswer,
            cancellationToken: cancellationToken);

        //    return;
        //}

        //await botClient.SendTextMessageAsync(
        //    chatId: update.Message.Chat.Id,
        //    text: CockAnswers.GetRejection(from.FirstName),
        //    cancellationToken: cancellationToken);
    }

    public async Task PrintCocksTopAsync(
        ITelegramBotClient botClient, Update update, string command, CancellationToken cancellationToken)
    {
        var chat = update.Message!.Chat;

        //if (chat.Type == ChatType.Private)
        //{
        //    await botClient.SendTextMessageAsync(
        //        chatId: update.Message.Chat.Id,
        //        text: CockAnswers.GetCocksTopPrivateChatRejection(),
        //        cancellationToken: cancellationToken);
        //    return;
        //}

        var accounts = (await _accountRepository.GetAllAccountsFromChatAsync(chat.Id, cancellationToken))
            .OrderByDescending(x => x.User.Cock.Length)
            .ToList();
        
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(CockAnswers.GetCocksTopStartingString());

        for (var i = 0; i < accounts.Count; i++)
        {
            var account = accounts[i];
            var user = accounts[i].User;

            stringBuilder.AppendLine(CockAnswers.GetCocksTopString(
                i + 1,
                user.HasIronCock,
                account.FirstName,
                account.LastName,
                user.Cock.Length));
        }
        
        await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: stringBuilder.ToString(),
            cancellationToken: cancellationToken);
    }

    public async Task PrintAbsoluteCockPositionAsync(
        ITelegramBotClient botClient, Update update, string command, CancellationToken cancellationToken)
    {
        var from = update.Message!.From!;

        var absoluteRatingNumber = (await _accountRepository.GetAllAccountsAsync(cancellationToken))
            .OrderByDescending(x => x.User.Cock.Length)
            .ToList()
            .FindIndex(x => x.Id == from.Id) + 1;

        await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: CockAnswers.GetCockAbsoluteString(absoluteRatingNumber),
            cancellationToken: cancellationToken);
    }
}