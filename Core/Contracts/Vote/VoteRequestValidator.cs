using FluentValidation;

namespace Core.Contracts.Vote;
public class VoteRequestValidator : AbstractValidator<VoteRequest>
{
    public VoteRequestValidator()
    {
        RuleFor(q => q.Answers).NotEmpty();
        RuleForEach(q => q.Answers)
            .SetInheritanceValidator(v => v.Add(new VoteAnswerRequestValidator()));

    }
}
