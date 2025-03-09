namespace SurvayBasket.Api.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);


        }
        catch (Exception)
        {
            _logger.LogError("An error has occured");
            ProblemDetails problemDetails = new ProblemDetails
            {

                Title = "An error has occured",
                Status = StatusCodes.Status500InternalServerError,
                Detail = "An error has occured",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
