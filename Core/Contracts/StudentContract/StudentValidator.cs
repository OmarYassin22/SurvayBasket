using FluentValidation;

namespace Core.Contracts.StudentContract;
public class StudentValidator : AbstractValidator<Student>
{
    public StudentValidator()
    {
        RuleFor(s => s.DateOfBirth)
        //.Must(s => s.Value.AddYears(18) < DateTime.Today)
        //.When(s => s.DateOfBirth.HasValue)
        //.WithMessage("You must be at least 18 years old to register")
        //.NotEmpty();
        .Must(BeValidAge);
    }
    private bool BeValidAge(DateTime? date)
    {
        if (date.HasValue)
        {
            return date.Value.AddYears(18) < DateTime.Today;
        }
        return false;
    }
}
