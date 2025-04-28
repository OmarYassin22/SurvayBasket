using FluentValidation;

namespace Core.Contracts.Vote;
public class VoteAnswerRequestValidator : AbstractValidator<VoteAnswerRequest>
{
    public VoteAnswerRequestValidator()
    {
        RuleFor(q => q.QuestionId).GreaterThan(0);
        RuleFor(q => q.AnswerId).GreaterThan(0);

    }
}
