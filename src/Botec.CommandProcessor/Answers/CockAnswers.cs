using Botec.CommandProcessor.Enums;

namespace Botec.CommandProcessor.Answers;

public class CockAnswers
{
    public static string GetCockUpdateAnswer(string firstName, string? lastName, int size) => size switch
    {
        >= 0 and < (int)CockConstants.Double => $"Кок {firstName} {lastName ?? string.Empty} вырос на {size} см!",
        < 0 and > (int)CockConstants.Circumcision => $"Кок {firstName} {lastName ?? string.Empty} объевреился на {size} см! Саси))0",
        (int)CockConstants.Double => $"Кок {firstName} {lastName ?? string.Empty} вырос в 2 раза!",
        (int)CockConstants.Circumcision => $"Кок {firstName} {lastName ?? string.Empty} отвалился! Лох))0)"
    };

    public static string GetResultCockLength(int length)
        => $"\nДлина кока: {length}";

    public static string GetRejection(string firstName)
        => $"{firstName} на сегодня хватит кока, залетай завтра - повторим";
}