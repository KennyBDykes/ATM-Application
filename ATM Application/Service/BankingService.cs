public class BankingService :IBankingService
{
    private IAccountRepository _accountRepository;
    private ITransactionRepository _transactionRepository;

    public BankingService(IAccountRepository accountRepository, ITransactionRepository transactionRepository)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
    }
   public decimal GetAccountBalance(string accountType)
    {
        var accountBalance = _accountRepository.GetAccountBalance(accountType);
        return accountBalance;
    }
   public IEnumerable<Transaction> GetTransactionHistory(string accountType)
    {
        var transactionHistory = _transactionRepository.GetTransactionHistory(accountType);
        return transactionHistory;
    }
   public void Deposit(RequestDto dto)
    {
        _accountRepository.Deposit(dto.AccountType, dto.Amount);
         var transaction = new Transaction {
          Date = DateTime.Now,
          Amount = dto.Amount,
          Type = "Deposit",
          AccountType = dto.AccountType
         };
        _transactionRepository.AddTransaction(transaction);

    }
   public void Withdrawal(RequestDto dto)
    {
        var balance = _accountRepository.GetAccountBalance(dto.AccountType);

        if(balance < dto.Amount)
        {
            throw new InvalidDataException("Not enough money in account to withdraw");
        }
        _accountRepository.Withdrawal(dto.AccountType, dto.Amount);
         var transaction = new Transaction {
          Date = DateTime.Now,
          Amount = dto.Amount,
          Type = "Withdrawal",
          AccountType = dto.AccountType
         };
        _transactionRepository.AddTransaction(transaction);
    }
   public void Transfer(TransferDto dto)
    {
        if(dto.AccountFrom == dto.AccountTo)
        {
            throw new InvalidDataException("Attempting to transfer from and to the same account");
        }
        var balance = _accountRepository.GetAccountBalance(dto.AccountFrom);
        if(balance < dto.Amount)
        {
            throw new InvalidDataException("Not enough money in account to transfer successfully");
        }
        _accountRepository.Transfer(dto.AccountFrom, dto.AccountTo, dto.Amount);
         var transactionfrom = new Transaction {
          Date = DateTime.Now,
          Amount = dto.Amount,
          AccountTo = dto.AccountTo,
          AccountFrom = dto.AccountFrom,
          Type = "Transfer",
          AccountType = dto.AccountFrom
         }; 
        _transactionRepository.AddTransaction(transactionfrom);


         var transactionto = new Transaction {
          Date = DateTime.Now,
          Amount = dto.Amount,
          AccountTo = dto.AccountTo,
          AccountFrom = dto.AccountFrom,
          Type = "Transfer",
          AccountType = dto.AccountTo
         }; 
        _transactionRepository.AddTransaction(transactionto);

    }

}