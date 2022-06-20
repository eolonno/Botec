using Botec.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Botec.Domain;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> User { get; set; }
    public DbSet<Cock> Cock { get; set; }
    public DbSet<Chat> Chat { get; set; }
    public DbSet<WhoIAm> WhoIAm { get; set; }
    public DbSet<Joke> Joke { get; set; }
    public DbSet<Nickname> Nickname { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Database=Botec;Trusted_Connection=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(x => x.Account)
            .WithOne(x => x.User);

        modelBuilder.Entity<WhoIAm>()
            .HasOne(x => x.Nickname);

        modelBuilder.Entity<Chat>()
            .HasOne(x => x.Faggot);
    }
}