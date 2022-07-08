using Botec.CommandProcessor.Answers;
using Botec.CommandProcessor.Utilities.Extensions;
using Botec.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Botec.CommandProcessor.CommandsLogic;

public class NicknameOfTheDayLogic
{
    private readonly IAccountRepository _accountRepository;
    private readonly INicknameRepository _nicknameRepository;

    public NicknameOfTheDayLogic(IServiceProvider services)
    {
        _accountRepository = services.GetRequiredService<IAccountRepository>();
        _nicknameRepository = services.GetRequiredService<INicknameRepository>();
    }

    public async Task PrintNicknameOfTheDay(
        ITelegramBotClient botClient, Update update, string command, CancellationToken cancellationToken)
    {
        var from = update.Message!.From!;

        var account = await _accountRepository.GetAccountByAccountIdAsync(from.Id, cancellationToken);
        var nicknameDay = account!.User.NicknameOfTheDay?.Day;

        if (nicknameDay != DateTime.Today)
        {
            var newNickname = (await _nicknameRepository.GetAllNicknames(cancellationToken)).GetRandomElement();
            await _nicknameRepository.UpdateNicknameOfTheDay(from.Id, newNickname.Id, cancellationToken);
        }

        var nickname = (await _accountRepository.GetAccountByAccountIdAsync(from.Id, cancellationToken))!
            .User.NicknameOfTheDay.Nickname!.Text;
        var introductoryPhrase = (await _nicknameRepository.GetAllNicknameIntroductoryPhrases(cancellationToken))
            .GetRandomElement().Text;

        await botClient.SendTextMessageAsync(
            chatId: update.Message!.Chat.Id,
            text: NicknameOfTheDayAnswers.GetNicknameOfTheDayString(from.FirstName, from.LastName, introductoryPhrase, nickname),
            cancellationToken: cancellationToken);
    }
}