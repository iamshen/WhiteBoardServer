using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Jwt;

/// <summary>
///     JwtBearer服务实现
/// </summary>
public class JwtService : IJwtService
{
    private const string Secret = "e276d4ab-2a93-4697-b6d4-ab2a9306978c";

    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    public string CreateToken(params Claim[] claims)
    {
        var now = DateTime.UtcNow;

        SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = credentials,
            IssuedAt = now,
            Expires = now.AddDays(7)
        };
        var token = _tokenHandler.CreateToken(descriptor);
        return _tokenHandler.WriteToken(token);
    }

    /// <summary>
    ///     读取声明
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public IEnumerable<Claim> ReadToken(string token)
    {
        var jwtToken = _tokenHandler.ReadJwtToken(token);
        return jwtToken.Claims;
    }
}