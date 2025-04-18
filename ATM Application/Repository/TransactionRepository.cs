public class TransactionRepository: ITransactionRepository
{
private IList <Transaction> _transactions;  
public TransactionRepository ()
{
    _transactions = new List <Transaction>();
}
public void AddTransaction (Transaction transaction)
{
    _transactions.Add(transaction);
}
public IEnumerable<Transaction> GetTransactionHistory(string accountType)
{
 return _transactions.Where(x => x.AccountType == accountType);
}
}