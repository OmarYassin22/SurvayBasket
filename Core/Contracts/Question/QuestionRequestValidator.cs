using FluentValidation;

namespace Core.Contracts.Question;
public class QuestionRequestValidator : AbstractValidator<QuestionRequest>
{
    public QuestionRequestValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(1000);
        RuleFor(x => x.Answers)
            .Must(x => x.Count > 1)
            .WithMessage("Question must have at least 2 answers");
        RuleFor(x => x.Answers)
            .Must(x => x.Distinct().Count() == x.Count)
            .WithMessage("Answers must be unique");
    }
}
