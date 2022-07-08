using Botec.CommandProcessor.Enums;
using Botec.CommandProcessor.Utilities.Extensions;

namespace Botec.CommandProcessor.Answers;

public class CockAnswers
{
    public static string GetCockUpdateAnswer(string firstName, string lastName, int size) => size switch
    {
        >= 0 and < (int)CockConstants.Double => $"Кок {firstName} {lastName} вырос на {size} см!".RemoveWhitespaces(),
        < 0 and > (int)CockConstants.Circumcision => $"Кок {firstName} {lastName} объевреился на {size} см! Саси))0".RemoveWhitespaces(),
        (int)CockConstants.Double => $"Кок {firstName} {lastName} вырос в 2 раза!".RemoveWhitespaces(),
        (int)CockConstants.Circumcision => $"Кок {firstName} {lastName} отвалился! Лох))0)".RemoveWhitespaces()
    };

    public static string GetResultCockLength(int length)
        => $"\nДлина кока: {length}";

    public static string GetRejection(string firstName)
        => $"{firstName} на сегодня хватит кока, залетай завтра - повторим";

    public static string GetCocksTopPrivateChatRejection()
        => "Ахаха. Ну ясен хрен, ты тут топ 1. Проверим в какой-нибудь беседе?))0";

    public static string GetCocksTopStartingString()
        => "Топ коков:";

    public static string GetCocksTopString(int number, bool hasIronCock, string firstName, string lastName, int length)
        => $"{number}. {firstName} {lastName } ({length} см)".RemoveWhitespaces();

    public static string GetCockAbsoluteString(int number)
        => $"Твое место в абсолютном рейтинге: {number}";
}