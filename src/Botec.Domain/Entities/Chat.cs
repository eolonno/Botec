using Botec.Domain.Enums;

namespace Botec.Domain.Entities;

public class Chat
{
    public Guid Id { get; set; }
    public long ChatId { get; set; }
    public MessengerType ChatType { get; set; }
    public User Faggot { get; set; }
    public DateTime Day { get; set; }
}