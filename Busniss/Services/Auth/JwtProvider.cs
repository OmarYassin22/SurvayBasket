using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SurvayBasket.Api.Options;

namespace SurveyBasket.Api.Services.Auth;
public class JwtProvider : IJwtProvider
{
    private readonly IConfiguration _configuration;
    private readonly JwtOptions _jwtOptions;

    public JwtProvider(IConfiguration configuration, IOptionsMonitor<JwtOptions> jwtOptions)
    {
        _configuration = configuration;
        _jwtOptions = jwtOptions.CurrentValue;
    }

    public string GenerateToken(ApplicationUser user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };



        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryInMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtOptions.Key);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero,

            }, out SecurityToken secureToken);

            var jwtToken = (JwtSecurityToken)secureToken;

            return jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value;


        }
        catch
        {

            return null;

        }

    }
}
