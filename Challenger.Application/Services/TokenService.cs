using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Challenger.Application.Configss;
using Challenger.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Challenger.Application.Services;

public class TokenService(JWTSettings jwtSettings) : ITokenService
{
    private readonly JWTSettings _jwtSettings = jwtSettings ?? throw new ArgumentNullException(nameof(jwtSettings));
    
    public string GenerateToken(User user)
    {
        var handler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var tokenDescription = new SecurityTokenDescriptor
        {
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddMinutes(10),
            Subject = GenerateClaims(user)
        };

        var jwt = handler.CreateToken(tokenDescription);
        return handler.WriteToken(jwt);
    }
    
    private static ClaimsIdentity GenerateClaims(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email.Valor)
        };
        
        return new ClaimsIdentity(claims);
    }
}