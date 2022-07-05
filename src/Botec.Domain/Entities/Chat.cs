using Botec.Domain.Enums;

namespace Botec.Domain.Entities;

public class Chat
{
    public long Id { get; set; }
    public MessengerType MessengerType { get; set; }
    public List<Account> Accounts { get; set; }
    public Account? FaggotOfTheDay { get; set; }
    public long? FaggotOfTheDayId { get; set; }
    public DateTime? LastFaggotChangeDate { get; set; }
}