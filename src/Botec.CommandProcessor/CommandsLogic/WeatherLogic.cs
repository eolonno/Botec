using Botec.CommandProcessor.Answers;
using Botec.CommandProcessor.Utilities.Extensions;
using OpenWeatherAPI;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Botec.CommandProcessor.CommandsLogic;

public class WeatherLogic
{
    private readonly string _token;

    public WeatherLogic(string token)
    {
        _token = token;
    }

    public async Task PrintWeatherAsync(
        ITelegramBotClient botClient, Update update, string command, CancellationToken cancellationToken)
    {
        var message = update.Message!.Text!;
        var commandEndIndex = command.Length;
        var city = message
            .Substring(commandEndIndex, message.Length - commandEndIndex)
            .Replace(" ", string.Empty);

        if (city == string.Empty)
            return;

        var weatherApi = new OpenWeatherApiClient(_token);
        QueryResponse weatherInfo;

        try
        {
            weatherInfo = await weatherApi.QueryAsync(city + "&lang=ru");
        }
        catch
        {
            await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: WeatherAnswers.GetWeatherRejection(),
                cancellationToken: cancellationToken);
            return;
        }
        
        await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: WeatherAnswers.GetWeatherString(weatherInfo, city),
            cancellationToken: cancellationToken);
    }
}