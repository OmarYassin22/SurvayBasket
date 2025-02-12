namespace SurveyBasket.Api.Services.Auth;
public interface IJwtProvider
{
    string GenerateToken(ApplicationUser user);
}
