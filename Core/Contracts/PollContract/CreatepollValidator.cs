using FluentValidation;

namespace Core.Contracts.Poll;
public class CreatePollValidator : AbstractValidator<CreatePollRequest>
{
    public CreatePollValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .Length(3, 100)
            .WithMessage("{PropertyName} must be min: {MinLength} max: {MaxLength}, you entered {TotalLength}");

        RuleFor(x => x.StartsAt)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("{PropertyName} must be more than today");
        RuleFor(x => x.EndsAt)
            .NotEmpty();
        RuleFor(x => x)
            .Must(BeAValidDate)
            .WithName(nameof(CreatePollRequest.EndsAt))
            .WithMessage("End date must be greater than or equal to start date.");
    }

    private bool BeAValidDate(CreatePollRequest request)
    {
        return request.EndsAt >= request.StartsAt;
    }
}
