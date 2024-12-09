namespace SurvayBasket.Api.MiddleWars;

public class CustomMiddleware : IMiddleware
{
    private readonly ILogger<CustomMiddleware> _logger;
    public CustomMiddleware(ILogger<CustomMiddleware> logger)
    {
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _logger.LogInformation("Custom Middleware start");
        await next(context);
        _logger.LogInformation("Custom Middleware end");
    }
}
