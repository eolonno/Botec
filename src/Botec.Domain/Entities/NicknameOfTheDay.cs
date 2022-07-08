namespace Botec.Domain.Entities;

public class NicknameOfTheDay
{
    public Guid Id { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
    public Nickname? Nickname { get; set; }
    public Guid? NicknameId { get; set; }
    public DateTime? Day { get; set; }
}