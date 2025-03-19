using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Data.Models;

public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string email)
        {
            var _context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext))!;
            bool emailExists = _context.Customers.Any(c => c.Email == email);

            if (emailExists)
                return new ValidationResult("Email ID already exists. Please use a different email.");
        }
        return ValidationResult.Success;
    }
}
public class StartDateBeforeEndDateAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is DateTime startDate)
        {
            var endDateProperty = validationContext.ObjectType.GetProperty("EndDate");
            if (endDateProperty != null)
            {
                var endDate = (DateTime)endDateProperty.GetValue(validationContext.ObjectInstance, null)!;
                if (startDate >= endDate)
                {
                    return new ValidationResult("Start Date must be earlier than End Date.");
                }
            }
        }
        return ValidationResult.Success;
    }
}
