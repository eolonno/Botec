using Botec.Domain.Enums;

namespace Botec.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public UserStatus UserStatus { get; set; }
    public bool HasIronCock { get; set; }
    public Cock Cock { get; set; }
    public NicknameOfTheDay NicknameOfTheDay { get; set; }
    public IEnumerable<Account> Accounts { get; set; }
}