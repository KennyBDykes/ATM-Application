using ATM_Application.Models;
using ATM_Application.Repositories;

namespace ATM_Application.Services
{
  public class BankingService :IBankingService
  {
    private IAccountRepository _accountRepository;
    private ITransactionRepository _transactionRepository;

    public BankingService(IAccountRepository accountRepository, ITransactionRepository transactionRepository)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
    }
   public async Task<decimal> GetAccountBalance(AccountType accountType)
    {
        var accountBalance = await _accountRepository.GetAccountBalance(MapAccountTypeToString(accountType));
        return accountBalance;
    }
   public async Task <IEnumerable<Transaction>> GetTransactionHistory(AccountType accountType)
    {
        var transactionHistory = await _transactionRepository.GetTransactionHistory(MapAccountTypeToString(accountType));
        return transactionHistory;
    }
   public async Task Deposit(RequestDto dto)
    {
        var accountType  = MapAccountTypeToString(dto.AccountType);
        await _accountRepository.Deposit(accountType, dto.Amount);
         var transaction = new Transaction {
          Date = DateTime.Now,
          Amount = dto.Amount,
          Type = "Deposit",
          AccountType = accountType
         };
       await  _transactionRepository.AddTransaction(transaction);

    }
   public async Task Withdrawal(RequestDto dto)
    {

        var accountType  = MapAccountTypeToString(dto.AccountType);
        var balance = await _accountRepository.GetAccountBalance(accountType);

        if(balance < dto.Amount)
        {
            throw new InvalidDataException("Not enough money in account to withdraw");
        }
        await _accountRepository.Withdrawal(accountType, dto.Amount);
         var transaction = new Transaction {
          Date = DateTime.Now,
          Amount = dto.Amount,
          Type = "Withdrawal",
          AccountType = accountType
         };
       await  _transactionRepository.AddTransaction(transaction);
    }
   public async Task Transfer(TransferDto dto)
    {
        if(dto.AccountFrom == dto.AccountTo)
        {
            throw new InvalidDataException("Attempting to transfer from and to the same account");
        }
        var acctFrom = MapAccountTypeToString(dto.AccountFrom);
        var acctTo = MapAccountTypeToString(dto.AccountTo);
        var balance = await _accountRepository.GetAccountBalance(acctFrom);
        if(balance < dto.Amount)
        {
            throw new InvalidDataException("Not enough money in account to transfer successfully");
        }
        await _accountRepository.Transfer(acctFrom, acctTo, dto.Amount);
         var transactionfrom = new Transaction {
          Date = DateTime.Now,
          Amount = dto.Amount,
          AccountTo = acctTo,
          AccountFrom = acctFrom,
          Type = "Transfer",
          AccountType = acctFrom
         }; 
       await  _transactionRepository.AddTransaction(transactionfrom);


         var transactionto = new Transaction {
          Date = DateTime.Now,
          Amount = dto.Amount,
          AccountTo = acctTo,
          AccountFrom = acctFrom,
          Type = "Transfer",
          AccountType = acctTo
         }; 
        await _transactionRepository.AddTransaction(transactionto);

    }
    private string MapAccountTypeToString(AccountType accountType)
    {
      switch (accountType)
        {
        case AccountType.Checking:
            return "Checking";
        case AccountType.Savings:
            return "Savings";
        default:
            return "Unknown Account Type";
         }
    }

  }
}