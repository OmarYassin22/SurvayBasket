
namespace SurvayBasket.Api.Controllers;
[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService, UserManager<ApplicationUser> userManager) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    [HttpPost("")]
    public async Task<IActionResult> GetToken([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);
       
        return authResult == null ? BadRequest("Invalid Email/Password") : Ok(authResult);
    }
}
