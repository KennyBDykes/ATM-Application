using System.ComponentModel.DataAnnotations;

public class AccountTypeValidationAttribute : ValidationAttribute
{ 
    public override bool IsValid(object value)
    {
        // Ensure the value is an enum of type AccountType
        if (value is AccountType accountType)
        {
            // If the value is a valid AccountType, return true
            return Enum.IsDefined(typeof(AccountType), accountType);
        }

        // If not an AccountType enum, validation fails
        return false;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} must be a valid account type (Checking or Savings).";
    }
}