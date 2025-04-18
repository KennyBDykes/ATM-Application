using System.ComponentModel.DataAnnotations;

public class AccountTypeValidationAttribute : ValidationAttribute
{
    private static readonly string[] AllowedTypes = { "Checking", "Savings" };

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string accountType)
        {
            if (AllowedTypes.Contains(accountType, StringComparer.OrdinalIgnoreCase))
            {
                return ValidationResult.Success;
            }
        }
        return new ValidationResult("AccountType must be either 'Checking' or 'Savings'.");
    }
}