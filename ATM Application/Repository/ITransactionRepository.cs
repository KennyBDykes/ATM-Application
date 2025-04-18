public interface ITransactionRepository
{
    void AddTransaction(Transaction transaction);
    IEnumerable<Transaction> GetTransactionHistory(string accountType);
}