using Microsoft.EntityFrameworkCore;

namespace ATM_Application.Repositories 
{
    public class AccountRepository : IAccountRepository
    {
      private BankingDbContext _context;
    public AccountRepository(BankingDbContext context)
{
  _context = context;
}
    public async Task Deposit(string accountType, decimal amount)
    {
       var account = await _context.Accounts.Where(x => x.AccountType == accountType).FirstOrDefaultAsync();
       if(account == null)
       {
         throw new ArgumentException("Account not found");
       }
        account.Balance += amount;

        await _context.SaveChangesAsync(); 
    }
    public async Task Withdrawal(string accountType, decimal amount)
    {
      var account = await _context.Accounts.Where(x => x.AccountType == accountType).FirstOrDefaultAsync();
      if(account == null)
       {
         throw new ArgumentException("Account not found");
       }
      account.Balance -= amount;

       await _context.SaveChangesAsync(); 
    }
    public async Task Transfer(string accountfrom , string accountTo, decimal amount)
    {
       using var transaction = await _context.Database.BeginTransactionAsync(); 

      try
      {
      var acctFrom = await _context.Accounts.Where(x => x.AccountType == accountfrom).FirstOrDefaultAsync();
      var acctTo = await _context.Accounts.Where(x => x.AccountType == accountTo).FirstOrDefaultAsync();
      if(acctFrom == null || acctTo == null)
      {
         throw new ArgumentException("Account not found");
      }
      acctFrom.Balance -= amount;
      acctTo.Balance += amount;
      await _context.SaveChangesAsync();
      await transaction.CommitAsync();
      } 
      catch(Exception e)
      {
       await transaction.RollbackAsync();   
       throw;
      }
    }
    public async Task<decimal> GetAccountBalance(string accountType)
    {
        return await _context.Accounts.Where(x => x.AccountType == accountType)
         .Select(x => x.Balance)
         .FirstOrDefaultAsync();
    } 
  } 
}
