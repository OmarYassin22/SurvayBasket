namespace Core.Contracts.Auth;
public record AuthResponse(
    string Id,
    string? Email,
    string FirstName,
    string LastName,
    string Token,
    int expiresIn,
    string RefreshToken,
    DateTime RefreshTokenExpiresIn
    );