﻿using Botec.Domain.Entities;
using Botec.Domain.Enums;
using Botec.Domain.Repositories;
using Telegram.Bot.Types;
using Chat = Botec.Domain.Entities.Chat;
using User = Botec.Domain.Entities.User;

namespace Botec.CommandProcessor.Services;

public class UserProcessingService
{
    private static readonly UserRepository _userRepository = new();
    private static readonly AccountRepository _accountRepository = new();
    private static readonly ChatRepository _chatRepository = new();

    public static async Task RegisterUserIfNotExistAsync(Update update, CancellationToken cancellationToken)
    {
        var from = update.Message!.From;

        if (from is null)
        {
            return;
        }

        var account = await _accountRepository.GetAccountByAccountIdAsync(from.Id, cancellationToken);
        var user = account is null ? null : await _userRepository.GetUserByAccountAsync(account, cancellationToken);


        if (user is null)
        {
            var userId = Guid.NewGuid();

            user = new User
            {
                Id = userId,
                UserStatus = UserStatus.Standard,
                HasIronCock = false,
                Cock = new Cock
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Length = 0
                },
                Accounts = new List<Account>
                {
                    new()
                    {
                        Id = from.Id,
                        MessengerType = MessengerType.Telegram,
                        FirstName = from.FirstName,
                        LastName = from.LastName,
                        Username = from.Username,
                    }
                },
            };

            await _userRepository.AddUserAsync(user, cancellationToken);
        }
    }

    public static async Task RegisterUserInTheChatIfNotRegisteredAsync(
        Update update, CancellationToken cancellationToken)
    {
        var from = update.Message!.From!;
        var chat = update.Message.Chat;

        var account = (await _accountRepository.GetAccountByAccountIdAsync(from.Id, cancellationToken))!;

        if (account.Chats.FirstOrDefault(x => x.Id == chat.Id) is not null)
        {
            return;
        }

        await _chatRepository.AddAccountToTheChatAsync(from.Id, chat.Id, cancellationToken);
    }
}