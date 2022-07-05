namespace Botec.CommandProcessor;

public class Commands
{
    public static readonly IEnumerable<string> RepeatCommands = new List<string> 
    { 
        "бот повтори",
        "бот, повтори",
        "бот скажи",
        "бот, скажи",
        "bot repeat"
    };

    public static readonly IEnumerable<string> UpdateCockCommands = new List<string>
    {
        "бот кок",
        "бот, кок",
        "bot cock"
    };

    public static readonly IEnumerable<string> CocksTopCommands = new List<string>
    {
        "бот топ коков",
        "бот, топ коков",
        "bot cocks top"
    };

    public static readonly IEnumerable<string> AbsoluteRatingCommands = new List<string>
    {
        "бот абсолют",
        "бот, абсолют",
        "bot absolute"
    };

    public static readonly IEnumerable<string> FaggotOfTheDayCommands = new List<string>
    {
        "бот пидор дня",
        "бот, пидор дня",
        "bot faggot of the day"
    };

    public static readonly IEnumerable<string> BanekJokeCommands = new List<string>
    {
        "бот анек",
        "бот, анек",
        "bot anek"
    };
}