using ATM_Application.Models;

namespace ATM_Application.Repositories
{
    public interface ITransactionRepository
    {
        Task AddTransaction(Transaction transaction);
        Task <IEnumerable<Transaction>> GetTransactionHistory(string accountType);
    }
}