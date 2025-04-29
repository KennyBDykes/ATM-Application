
namespace ATM_Application.Repositories
{
public interface IAccountRepository
{
    Task Deposit(string accountType, decimal amount);
    Task Withdrawal(string accountType, decimal amount);
    Task Transfer(string accountfrom , string accountTo, decimal amount);
    Task <decimal> GetAccountBalance(string accountType);
}
}
        