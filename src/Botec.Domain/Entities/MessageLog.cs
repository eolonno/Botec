using Botec.Domain.Enums;

namespace Botec.Domain.Entities;

public class MessageLog
{
    public Guid Id { get; set; }
    public long AccountId { get; set; }
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public long ChatId { get; set; }
    public string Text { get; set; }
    public MessengerType MessengerType { get; set; }
    public DateTime Date { get; set; }
}