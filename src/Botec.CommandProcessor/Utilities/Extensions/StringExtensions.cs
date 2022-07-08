using System.Text.RegularExpressions;

namespace Botec.CommandProcessor.Utilities.Extensions;

public static class StringExtensions
{
    public static string RemoveWhitespaces(this string str)
    {
        return Regex.Replace(str, @"\s+", " ");
    }
}