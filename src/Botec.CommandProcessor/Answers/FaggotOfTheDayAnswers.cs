using Botec.CommandProcessor.Utilities.Extensions;

namespace Botec.CommandProcessor.Answers;

public class FaggotOfTheDayAnswers
{
    public static string GetFaggotOfTheDayString(string firstName, string lastName)
        => $"Пидор дня: {firstName} {lastName}".RemoveWhitespaces();
}