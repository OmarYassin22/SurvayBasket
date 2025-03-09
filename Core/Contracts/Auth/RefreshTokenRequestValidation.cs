using FluentValidation;

namespace Core.Contracts.Auth;
public class RefreshTokenRequestValidation : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidation()
    {

        RuleFor(x => x.token)
            .NotEmpty();
        RuleFor(x => x.refreshToken)
            .NotEmpty();
    }
}
