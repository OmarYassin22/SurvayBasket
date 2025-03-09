using Core.Abestraction;
using Core.Contracts.Auth;
using OneOf;

namespace Busniss.Services.Auth;
public interface IAuthService
{

    Task<OneOf<AuthResponse, Error>> GetTokenAsync(string email, string password, CancellationToken cancellationToken);
    Task<OneOf<AuthResponse, Error>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken);
    Task<OneOf<bool, Error>> RefreshTokenRevokedAsync(string token, string refreshToken, CancellationToken cancellationToken);
}
