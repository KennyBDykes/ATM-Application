using System.Threading.Tasks;
using ATM_Application.Models;
using ATM_Application.Repositories; 
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;  // For LINQ queries (Where, FirstOrDefault, etc.)
using Xunit;

public class TransactionRepositoryTests
{
    private readonly TransactionRepository _repository;
    private readonly BankingDbContext _context;

    public TransactionRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<BankingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new BankingDbContext(options);
        _repository = new TransactionRepository(_context);
    }

    [Fact]
    public async Task AddTransaction_ShouldSaveTransaction()
    {
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountType = "Checking",
            Amount = 100,
            Type = "Deposit",
            Date = DateTime.UtcNow
        };

        await _repository.AddTransaction(transaction);

        var saved = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == transaction.Id);
        Assert.NotNull(saved);
        Assert.Equal("Deposit", saved.Type);
    }

    [Fact]
    public async Task GetTransactionsForAccount_ShouldReturnCorrectTransactions()
    {
        _context.Transactions.AddRange(
            new Transaction { Id = Guid.NewGuid(), AccountType = "Checking", Type = "Deposit", Amount = 100, Date = DateTime.UtcNow },
            new Transaction { Id = Guid.NewGuid(), AccountType = "Checking", Type = "Withdrawal", Amount = 50, Date = DateTime.UtcNow },
            new Transaction { Id = Guid.NewGuid(), AccountType = "Savings", Type = "Deposit", Amount = 200, Date = DateTime.UtcNow }
        );
        await _context.SaveChangesAsync();

       var result = await _repository.GetTransactionHistory("Checking");

        Assert.All(result, t =>
      {
        Assert.Equal("Checking", t.AccountType);
        Assert.True(t.Amount > 0, "Amount should be greater than zero");
        Assert.Contains(t.Type, new[] { "Deposit", "Withdrawal" });
      });
    }
}