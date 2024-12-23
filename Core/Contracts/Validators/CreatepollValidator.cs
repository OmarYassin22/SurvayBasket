using FluentValidation;
using SurvayBasket.Api.DTO.Request;

namespace Core.Contracts.Validators;
public class CreatePollValidator : AbstractValidator<CreatePoll>
{
    public CreatePollValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty();
    }

}
