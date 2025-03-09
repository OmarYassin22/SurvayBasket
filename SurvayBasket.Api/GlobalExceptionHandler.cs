using Microsoft.AspNetCore.Diagnostics;

namespace SurvayBasket.Api;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {

        _logger.LogError(exception, "An error has occured");
        ProblemDetails problemDetails = new ProblemDetails
        {

            Title = "An error has occured",
            Status = StatusCodes.Status500InternalServerError,
            Detail = "An error has occured",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
        return true;
    }
}
