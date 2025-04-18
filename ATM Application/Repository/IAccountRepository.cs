using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.SignalR;

public interface IAccountRepository
{
    void Deposit(string accountType, decimal amount);
    void Withdrawal(string accountType, decimal amount);
    void Transfer(string accountfrom , string accountTo, decimal amount);
    decimal GetAccountBalance(string accountType);
}