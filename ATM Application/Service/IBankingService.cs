public interface IBankingService
{
    Task Deposit(RequestDto dto);
    Task Withdrawal(RequestDto dto);
    Task Transfer(TransferDto dto);
    Task <decimal> GetAccountBalance(AccountType accountType);
    Task<IEnumerable<Transaction>> GetTransactionHistory(AccountType accountType);

}