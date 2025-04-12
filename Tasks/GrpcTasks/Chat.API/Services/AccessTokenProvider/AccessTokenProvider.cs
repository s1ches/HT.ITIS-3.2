using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Chat.API.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Chat.API.Services.AccessTokenProvider;

public class AccessTokenProvider(IOptions<JwtOptions> jwtOptions) : IAccessTokenProvider
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    
    public string GetAccessToken(string userName)
    {
        var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
        
        var jwt = new JwtSecurityToken(issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenLifetimeMinutes),
            claims: [new Claim(ClaimTypes.Name, userName)],
            signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}