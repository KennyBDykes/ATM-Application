using System.Threading.Tasks;
using ATM_Application.Models;
using ATM_Application.Repositories; 
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;


    public class AccountRepositoryTests
    {
        private readonly BankingDbContext _context;
        private readonly AccountRepository _repository;

        public AccountRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<BankingDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            _context = new BankingDbContext(options);
            _repository = new AccountRepository(_context);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            _context.Accounts.Add(new Account
            {
                Id = Guid.NewGuid(),
                AccountType = "Checking",
                Balance = 500
            });

            _context.Accounts.Add(new Account
            {
                Id = Guid.NewGuid(),
                AccountType = "Savings",
                Balance = 1000
            });

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAccountBalance_ShouldReturnCorrectBalance()
        {
            var balance = await _repository.GetAccountBalance("Checking");

            Assert.Equal(500, balance);
        }
        

        [Fact]
        public async Task Deposit_ShouldIncreaseBalance()
        {
            await _repository.Deposit("Savings", 200);

            var updated = await _context.Accounts.FirstAsync(a => a.AccountType == "Savings");

            Assert.Equal(1200, updated.Balance);
        }

        [Fact]
        public async Task Withdrawal_ShouldDecreaseBalance()
        {
            await _repository.Withdrawal("Checking", 100);

            var updated = await _context.Accounts.FirstAsync(a => a.AccountType == "Checking");

            Assert.Equal(400, updated.Balance);
        }

        [Fact]
        public async Task Transfer_ShouldMoveFundsBetweenAccounts()
        {
            await _repository.Transfer("Savings", "Checking", 300);

            var savings = await _context.Accounts.FirstAsync(a => a.AccountType == "Savings");
            var checking = await _context.Accounts.FirstAsync(a => a.AccountType == "Checking");

            Assert.Equal(700, savings.Balance);
            Assert.Equal(800, checking.Balance);
        }
    
    }
