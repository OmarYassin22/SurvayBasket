namespace SurvayBasket.Api.ValidationAttributes;
using System.ComponentModel.DataAnnotations;


//[AttributeUsage(AttributeTargets.All)] --> This is the default value and use for all properties, classes, methods, records, etc.
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class MinAgeAttribute(int MinAge, int MaxAge) : ValidationAttribute
{
    //public override bool IsValid(object? value)
    //{
    //    if (value is DateTime date)
    //    {


    //        if (DateTime.Today < date.AddYears(MinAge))
    //        {
    //            return false;
    //        }
    //        else if (DateTime.Now.Year- date.Year > MaxAge)
    //        {
    //            return false;

    //        }
    //    }
    //    return true;
    //}
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {

        if (value is DateTime date)
        {


            if (DateTime.Today < date.AddYears(MinAge))
            {
                return new ValidationResult(errorMessage:$"Invalid {validationContext.DisplayName},  You are too young");
            }
            else if (DateTime.Now.Year - date.Year > MaxAge)
            {
                return new ValidationResult(errorMessage: $"Invalid {validationContext.DisplayName}, You are too old");

            }
        }
        return ValidationResult.Success;
    }
}
