using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class Account
{
    public Guid Id{ get; set; }= Guid.NewGuid();
    [Required]
    public string AccountType {get; set;}
    public decimal Balance { get; set; }

}