using System.ComponentModel.DataAnnotations;

public class RequestDto
{
    [Required]
    [AccountTypeValidation]
    public string AccountType { get; set; }
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public decimal Amount { get; set; }
}