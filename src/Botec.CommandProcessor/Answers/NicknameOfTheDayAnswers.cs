using Botec.CommandProcessor.Utilities.Extensions;

namespace Botec.CommandProcessor.Answers;

public class NicknameOfTheDayAnswers
{
    public static string GetNicknameOfTheDayString(string firstName, string lastName, string introductoryPhrase, string nickname)
        => $"Сегодня {firstName} {lastName} {introductoryPhrase} {nickname}".RemoveWhitespaces();
}