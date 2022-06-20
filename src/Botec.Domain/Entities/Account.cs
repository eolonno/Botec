using Botec.Domain.Enums;

namespace Botec.Domain.Entities;

public class Account
{
    public Guid Id { get; set; }
    public MessengerType MessengerType { get; set; }
    public long AccountId { get; set; }
    public User User { get; set; }
}