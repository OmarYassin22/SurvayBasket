using System.Security.Claims;

namespace SurvayBasket.Api.Extions;

public static class UserExtions
{
    public static string? GetuserId(this ClaimsPrincipal user) => user.FindFirstValue(ClaimTypes.NameIdentifier);
}
