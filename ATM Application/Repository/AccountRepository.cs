public class AccountRepository : IAccountRepository
{
private IDictionary<string, Account> _accounts;  
public AccountRepository()
{
 _accounts = new Dictionary<string, Account>
 {
     { "Checking", new Account{ AccountType = "Checking" , Balance = 0.0m} },
     { "Savings", new Account { AccountType = "Savings", Balance = 0.0m} }
 };
}
public void Deposit(string accountType, decimal amount)
{
  if(!_accounts.TryGetValue(accountType, out Account? account))
  {
    throw new ArgumentException("Account not found");
  }
  account.Balance += amount;
}
public void Withdrawal(string accountType, decimal amount)
{
 if(!_accounts.TryGetValue(accountType, out Account? account))
  {
    throw new ArgumentException("Account not found");
  }
  account.Balance -= amount;
}
public void Transfer(string accountfrom , string accountTo, decimal amount)
{
   if(!_accounts.TryGetValue(accountfrom, out Account? acctFrom))
  {
    throw new ArgumentException("Account not found");
  }
  if(!_accounts.TryGetValue(accountTo, out Account? acctTo))
  {
    throw new ArgumentException("Account not found");
  }
  acctFrom.Balance -= amount;
  acctTo.Balance += amount;
}
public decimal GetAccountBalance(string accountType)
{
if(!_accounts.TryGetValue(accountType, out Account? account))
  {
    throw new ArgumentException("Account not found");
  }
  return account.Balance;
}

}