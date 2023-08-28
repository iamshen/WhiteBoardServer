using System.Security.Claims;

namespace Infrastructure.Jwt;

public interface IJwtService
{
    /// <summary>
    ///     创建指定声明的 Jwt Token 信息
    /// </summary>
    /// <returns>JwtToken信息</returns>
    string CreateToken(params Claim[] claims);

    /// <summary>
    ///     获取声明
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    IEnumerable<Claim> ReadToken(string token);
}