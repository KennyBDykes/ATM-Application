public interface ITransactionRepository
{
    Task AddTransaction(Transaction transaction);
    Task <IEnumerable<Transaction>> GetTransactionHistory(string accountType);
}