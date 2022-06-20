﻿using Telegram.Bot;
using Telegram.Bot.Types;

namespace Botec.CommandProcessor.CommandsLogic;

public static class RepeatLogic
{
    public static async Task RepeatAsync(
        ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string command)
    {
        var message = update.Message!.Text!;
        var commandEndIndex = command.Length;
        var messageToSend = message.Substring(commandEndIndex, message.Length - commandEndIndex);

        if (messageToSend == string.Empty)
            return;

        await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: messageToSend,
            cancellationToken: cancellationToken);
    }
}