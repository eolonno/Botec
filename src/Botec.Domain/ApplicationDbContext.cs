using Botec.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Botec.Domain;

public class ApplicationDbContext : DbContext
{
    public DbSet<Account> Account { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<Chat> Chat { get; set; }
    public DbSet<Cock> Cock { get; set; }
    public DbSet<NicknameOfTheDay> NicknameOfTheDay { get; set; }
    public DbSet<Nickname> Nickname { get; set; }
    public DbSet<Joke> Joke { get; set; }
    public DbSet<MessageLog> MessageLog { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(x => x.Cock)
            .WithOne(x => x.User)
            .HasForeignKey<Cock>(x => x.UserId);
        modelBuilder.Entity<User>()
            .HasOne(x => x.NicknameOfTheDay)
            .WithOne(x => x.User)
            .HasForeignKey<NicknameOfTheDay>(x => x.UserId);
        modelBuilder.Entity<User>()
            .HasMany(x => x.Accounts)
            .WithOne(x => x.User);

        modelBuilder.Entity<Chat>()
            .HasOne(x => x.FaggotOfTheDay);
        modelBuilder.Entity<Chat>()
            .HasMany(x => x.Accounts)
            .WithMany(x => x.Chats);

        modelBuilder.Entity<NicknameOfTheDay>()
            .HasOne(x => x.Nickname);

        modelBuilder.Entity<Cock>()
            .HasOne(x => x.User)
            .WithOne(x => x.Cock);
    }
}