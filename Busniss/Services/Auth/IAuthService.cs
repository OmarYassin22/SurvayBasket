using Core.Contracts.Auth;

namespace Busniss.Services.Auth;
public interface IAuthService
{

    Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken);
}
