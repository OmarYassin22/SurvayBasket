namespace Core.Contracts.Auth;
public record RefreshTokenRequest(
    string token,
    string refreshToken
    );
