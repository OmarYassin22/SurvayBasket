namespace SurvayBasket.Api.MiddleWars;

public static class CustomMiddlewareExtension
{
    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CustomMiddleware>();

    }
}
