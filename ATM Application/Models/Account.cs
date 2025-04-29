using System.ComponentModel.DataAnnotations;

namespace ATM_Application.Models
{
    public class Account
    {
    public Guid Id{ get; set; }= Guid.NewGuid();
    [Required]
    public string AccountType {get; set;}
    public decimal Balance { get; set; }

    }
}