using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.SignalR;

public interface IAccountRepository
{
    Task Deposit(string accountType, decimal amount);
    Task Withdrawal(string accountType, decimal amount);
    Task Transfer(string accountfrom , string accountTo, decimal amount);
    Task <decimal> GetAccountBalance(string accountType);
}