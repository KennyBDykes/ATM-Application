using Microsoft.EntityFrameworkCore;

public class BankingDbContext : DbContext
{
    public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options) { }

    public DbSet<Account> Accounts { get; set; } 

    public DbSet<Transaction> Transactions { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
     modelBuilder.Entity<Account>(entity =>
    {
        entity.HasKey(a => a.Id); 
        entity.Property(a => a.Id)
              .HasDefaultValueSql("NEWID()");

        entity.Property(a => a.AccountType)
              .IsRequired();
    });

    modelBuilder.Entity<Transaction>(entity =>
    {
        entity.HasKey(t => t.Id); 
        entity.Property(t => t.Id)
              .HasDefaultValueSql("NEWID()");
    });

     // Seeding Checking and Savings Accounts
    modelBuilder.Entity<Account>().HasData(
    new Account
    {
        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
        AccountType = "Checking",
        Balance = 0m
    },
    new Account
    {
        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
        AccountType = "Savings",
        Balance = 0m
    });
    }
     
}