using Core.Abestraction;
using Core.Abestraction.Errors;
using Core.Contracts.Auth;
using Microsoft.AspNetCore.Identity;
using OneOf;
using SurveyBasket.Api.Services.Auth;
using System.Security.Cryptography;

namespace Busniss.Services.Auth;
public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private static readonly int _refreshTokenExpiryInDays = 30;

    public async Task<OneOf<AuthResponse, Error>> GetTokenAsync(string email, string password, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return UserErrors.InvaliedCredintioals!;
        var isValid = await _userManager.CheckPasswordAsync(user, password);

        var token = _jwtProvider.GenerateToken(user);
        var userId = _jwtProvider.ValidateToken(token);
        var refreshToken = GenerateResfreshToken();
        var expiryDate = DateTime.UtcNow.AddDays(_refreshTokenExpiryInDays);
        user.RefreshTokens.Add(new RefreshToken() { Token = refreshToken, ExpiresOn = expiryDate });
        await _userManager.UpdateAsync(user);

        return isValid
            ? new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, 30 * 80, refreshToken, expiryDate)
            : UserErrors.InvaliedCredintioals;
    }


    public async Task<OneOf<AuthResponse, Error>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken)
    {
        #region check if token is valid and refresh token is valid

        // 1- get userid from token
        var userId = _jwtProvider.ValidateToken(token);
        if (userId == null)
            return UserErrors.InvaliedCredintioals;
        // 2- get userid from DB
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return UserErrors.InvaliedCredintioals;
        // 3- get refresh token of user from DB
        var storedRefreshToken = user.RefreshTokens.SingleOrDefault(u => u.Token == refreshToken && u.IsActive);
        if (storedRefreshToken == null) return UserErrors.InvaliedCredintioals;
        // 4- check if refresh token is expired
        if (storedRefreshToken.ExpiresOn < DateTime.UtcNow) return UserErrors.InvaliedCredintioals;
        #endregion


        #region create new token and refresh token and revoked old one
        // 1- revoke old refresh token
        storedRefreshToken.RevokedOn = DateTime.UtcNow;
        // 2- generate new refresh token
        var newRefreshToken = GenerateResfreshToken();
        var expiryDate = DateTime.UtcNow.AddDays(_refreshTokenExpiryInDays);
        // 3- save new refresh token
        user.RefreshTokens.Add(new RefreshToken() { ExpiresOn = expiryDate, Token = newRefreshToken });
        await _userManager.UpdateAsync(user);
        // 4- generate new token
        var newToken = _jwtProvider.GenerateToken(user);
        #endregion




        return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newToken, 30 * 80, newRefreshToken, expiryDate);
    }
    public async Task<OneOf<bool, Error>> RefreshTokenRevokedAsync(string token, string refreshToken, CancellationToken cancellationToken)
    {
        var userId = _jwtProvider.ValidateToken(token);
        if (userId == null)
            return false;
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return false;
        var storedRefreshToken = user.RefreshTokens.SingleOrDefault(u => u.Token == refreshToken && u.IsActive);
        if (storedRefreshToken is null)
            return false;
        storedRefreshToken.RevokedOn = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);
        return true;

    }
    private string GenerateResfreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }


}
