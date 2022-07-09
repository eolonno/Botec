using Botec.CommandProcessor.Utilities.Extensions;
using OpenWeatherAPI;

namespace Botec.CommandProcessor.Answers;

public class WeatherAnswers
{
    public static string GetWeatherString(QueryResponse weatherInfo, string city)
        => $"Место: {city.FirstCharToUpper()}, {weatherInfo.Sys.Country}\n" +
           $"Температура: {weatherInfo.Main.Temperature.CelsiusCurrent}°C\n" +
           $"В окне: {weatherInfo.WeatherList.First().Description}\n" +
           $"Влажность трусиков: {weatherInfo.Main.Humidity}%\n" +
           $"Дует: {weatherInfo.Wind.SpeedMetersPerSecond}м/с\n";

    public static string GetWeatherRejection()
        => "Возникла какая-то ебучая ошибка. Похуй какая, главное, что ты И-Д-И-О-Т!";
}