namespace Botec.Domain.Entities;

public class WhoIAm
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Nickname Nickname { get; set; }
    public DateTime Day { get; set; }
}