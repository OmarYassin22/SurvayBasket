using Core.Contracts.Auth;
using Microsoft.AspNetCore.Identity;
using SurveyBasket.Api.Services.Auth;

namespace Busniss.Services.Auth;
public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return null;
        var isValid = await _userManager.CheckPasswordAsync(user, password);

        var token = _jwtProvider.GenerateToken(user);
        return isValid ? new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, 30 * 80) : null;
    }
}
