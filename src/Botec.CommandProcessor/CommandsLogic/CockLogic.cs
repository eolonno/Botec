using System.Text;
using Botec.CommandProcessor.Answers;
using Botec.CommandProcessor.Enums;
using Botec.Domain.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using Botec.CommandProcessor.Utilities;
using Botec.Domain.Enums;
using Telegram.Bot.Types.Enums;

namespace Botec.CommandProcessor.CommandsLogic;

public class CockLogic
{
    private static CockRepository _cockRepository;
    private static UserRepository _userRepository;

    static CockLogic()
    {
        _cockRepository = new CockRepository();
        _userRepository = new UserRepository();
    }

    public static async Task UpdateCockAsync(
        ITelegramBotClient botClient, Update update, string command, CancellationToken cancellationToken)
    {
        var from = update.Message!.From;
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

    public static async Task PrintCocksTop(
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

        var users = (await _userRepository.GetAllUsersFromChat(chat.Id, cancellationToken)).ToList();
        
        var stringBuilder = new StringBuilder();

        for (var i = 0; i < users.Count; i++)
        {
            var user = users[i];
            var userAccount = user.Accounts.Where(x => x.MessengerType == MessengerType.Telegram).FirstOrDefault();

            if (userAccount is null)
            {
                continue;
            }

            stringBuilder.Append(CockAnswers.GetCocksTopString(
                i + 1,
                user.HasIronCock,
                userAccount.FirstName,
                userAccount.LastName,
                user.Cock.Length));
        }
        
        await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: stringBuilder.ToString(),
            cancellationToken: cancellationToken);
    }
}