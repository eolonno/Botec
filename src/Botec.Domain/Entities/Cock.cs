namespace Botec.Domain.Entities;

public class Cock
{
    public Guid Id { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
    public int Length { get; set; }
    public DateTime? LastCommitDate { get; set; }
}