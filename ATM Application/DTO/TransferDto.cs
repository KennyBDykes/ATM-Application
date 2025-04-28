using System.ComponentModel.DataAnnotations;
public class TransferDto
{
    [Required]
    [AccountTypeValidation]
    public AccountType AccountFrom { get; set;}
    [Required]
    [AccountTypeValidation]
    public AccountType AccountTo { get; set; }
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public decimal Amount { get; set; }
}