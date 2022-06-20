namespace Botec.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public Cock Cock { get; set; }
    public WhoIAm WhoIAm { get; set; }
    public IEnumerable<Account> Account { get; set; }
}