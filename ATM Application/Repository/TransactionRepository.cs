using ATM_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace ATM_Application.Repositories
{
public class TransactionRepository: ITransactionRepository
{
    private BankingDbContext _context;
    public TransactionRepository (BankingDbContext context)
    {
        _context = context;
    }
    public async Task AddTransaction (Transaction transaction)
    {
        await  _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }
    public async Task<IEnumerable<Transaction>> GetTransactionHistory(string accountType)
    {
       return await  _context.Transactions.Where(t => t.AccountType == accountType).ToListAsync();
    }
}
}