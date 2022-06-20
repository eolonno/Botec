namespace Botec.Domain.Entities;

public class UserSettings
{
    public Guid Id { get; set; }
    public User User { get; set; }
    public bool IsAdmin { get; set; }
    public bool HasIronCock { get; set; }
}