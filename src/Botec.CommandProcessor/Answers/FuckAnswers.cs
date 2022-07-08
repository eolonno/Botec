using Botec.CommandProcessor.Utilities.Extensions;

namespace Botec.CommandProcessor.Answers;

public class FuckAnswers
{
    public static string GetFuckString(string fromFirstName, string fromLastName, string whoFirstName, string whoLastName)
        => $"{fromFirstName} {fromLastName} трахнул {whoFirstName} {whoLastName ?? string.Empty}".RemoveWhitespaces();

    public static string GetFuckHimselfString(string firstName, string lastName)
        => $"{firstName} {lastName} никого не выебал, просто подрочил".RemoveWhitespaces();
}