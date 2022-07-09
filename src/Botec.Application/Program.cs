using Botec.Application;
using Botec.CommandProcessor.Interfaces;
using Botec.CommandProcessor.Services;
using Botec.Domain;
using Botec.Domain.Interfaces;
using Botec.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

using var host = Host.CreateDefaultBuilder()
    .ConfigureServices(services =>
        services
            .AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration["ConnectionString"]))
            .AddSingleton<IChatProcessingService, ChatProcessingService>()
            .AddSingleton<IUserProcessingService, UserProcessingService>()
            .AddScoped<IAccountRepository, AccountRepository>()
            .AddScoped<IChatRepository, ChatRepository>()
            .AddScoped<ICockRepository, CockRepository>()
            .AddScoped<IMessageLogRepository, MessageLogRepository>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<INicknameRepository, NicknameRepository>()
            .AddScoped<IJokeRepository, JokeRepository>())
    .ConfigureLogging(logging => 
        logging
            .AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning))
    .Build();

var botClient = new TelegramBotClient(configuration["TelegramToken"]);
var messageProcessingService = new MessageProcessingService(host.Services, configuration);

using var cancellationToken = new CancellationTokenSource();

var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = Array.Empty<UpdateType>()
};
botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cancellationToken.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

cancellationToken.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Message is not {} message)
        return;
    if (message.Text is not {})
        return;

    await messageProcessingService.ProcessMessage(botClient, update, cancellationToken);
}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var errorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(errorMessage);
    return Task.CompletedTask;
}