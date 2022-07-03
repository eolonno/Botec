using Botec.Domain.Enums;

namespace Botec.Domain.Entities;

public class Chat
{
    public long Id { get; set; }
    public MessengerType MessengerType { get; set; }
    public User? FaggotOfTheDay { get; set; }
    public DateTime? LastFaggotChangeDate { get; set; }
}