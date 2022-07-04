using Botec.Domain.Enums;

namespace Botec.Domain.Entities;

public class Account
{
    public long Id { get; set; }
    public MessengerType MessengerType { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Username { get; set; }
    public User User { get; set; }
    public List<Chat> Chats { get; set; }
}