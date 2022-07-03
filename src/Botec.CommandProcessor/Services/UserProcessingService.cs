using Botec.Domain.Entities;
using Botec.Domain.Enums;
using Botec.Domain.Repositories;
using Telegram.Bot.Types;
using User = Botec.Domain.Entities.User;

namespace Botec.CommandProcessor.Services;

public class UserProcessingService
{
    private static readonly UserRepository _userRepository;
    private static readonly AccountRepository _accountRepository;

    static UserProcessingService()
    {
        _userRepository = new UserRepository();
        _accountRepository = new AccountRepository();
    }

    public static async Task RegisterUserIfNotExistAsync(Update update, CancellationToken cancellationToken)
    {
        var from = update.Message!.From;

        if (from is null)
        {
            return;
        }

        var account = await _accountRepository.GetAccountByAccountId(from.Id, cancellationToken);
        var user = account is null ? null : await _userRepository.GetUserByAccountAsync(account, cancellationToken);
        

        if (user is null)
        {
            var userId = Guid.NewGuid();

            user = new User
            {
                Id = userId,
                Name = from.FirstName,
                LastName = from.FirstName,
                UserName = from.Username,
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
                        Id = Guid.NewGuid(),
                        AccountId = from.Id,
                        MessengerType = MessengerType.Telegram
                    }
                }
            };

            user.Accounts.First().User = user;

            await _userRepository.AddUserAsync(user, cancellationToken);
        }
    }
}