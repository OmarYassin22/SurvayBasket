using Core.Contracts.Auth;

namespace SurvayBasket.Api.Controllers;
[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService, UserManager<ApplicationUser> userManager, ILogger<AuthController> logger) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ILogger<AuthController> _logger = logger;

    [HttpPost("")]
    public async Task<IActionResult> GetToken([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Welcom From Auth Controller");
        var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

        return authResult.Match<IActionResult>(
           Ok,
            error => BadRequest(error)
            );
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> GetRefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetRefreshTokenAsync(request.token, request.refreshToken, cancellationToken);

        return authResult.Match<IActionResult>(
            Ok,
             error => BadRequest(error)
             );
    }
    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var revokeResult = await _authService.RefreshTokenRevokedAsync(request.token, request.refreshToken, cancellationToken);
        return revokeResult.Match<IActionResult>(authResponse => Ok(true), error => BadRequest("Invalid Operation"));

    }
}
