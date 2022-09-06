using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using leave_a_note_core.Models.Authentication;
using leave_a_note_core.Models.Authentication.Responses;
using leave_a_note_core.Models.DTOs;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace leave_a_note_core.Services.Authenticators;

public class Authenticator
{
    private readonly AuthenticationConfiguration _configuration;

    public Authenticator(IOptions<AuthenticationConfiguration> configuration)
    {
        _configuration = configuration.Value;
    }

    public AuthenticatedUserResponse Authenticate(UserLoginDto user)
    {
        var accessToken = GenerateAccessToken(user);

        return new AuthenticatedUserResponse
        {
            AccessToken = accessToken
        };
    }

    public string GenerateAccessToken(UserLoginDto user)
    {
        var claims = new List<Claim>
        {
            new Claim("id", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };

        return GenerateToken(
            _configuration.AccessTokenSecret,
            _configuration.Issuer,
            _configuration.Audience,
            _configuration.AccessTokenExpirationMinutes,
            claims);
    }

    public string GenerateToken(
        string secretKey, 
        string issuer, 
        string audience, 
        double expirationMinutes,
        IEnumerable<Claim>? claims = null)
    {
        SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(expirationMinutes),
            credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
