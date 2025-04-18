public interface IBankingService
{
    void Deposit(RequestDto dto);
    void Withdrawal(RequestDto dto);
    void Transfer(TransferDto dto);
    decimal GetAccountBalance(string accountType);
    IEnumerable<Transaction> GetTransactionHistory(string accountType);

}