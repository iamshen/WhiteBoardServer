using Infrastructure.Constants;
using Infrastructure.Dtos.Room;
using Infrastructure.Jwt;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.SignalR;

/// <summary>
///     Extension methods for accessing <see cref="RoomInfo" /> from a hub context.
/// </summary>
public static class GetRoomExtensions
{
    /// <summary>
    ///     Gets <see cref="RoomInfo" /> from the specified connection, or <c>null</c> if the connection is not associated
    ///     with an HTTP request.
    /// </summary>
    /// <param name="connection">The connection.</param>
    /// <returns>
    ///     The <see cref="HttpContext" /> for the connection, or <c>null</c> if the connection is not associated with an
    ///     HTTP request.
    /// </returns>
    public static async Task<RoomInfo?> GetRoomAsync(this HubCallerContext connection)
    {
        var httpContext = connection.GetHttpContext();
        if (httpContext is null)
            return default;

        // 获取 RoomToken
        var roomToken = httpContext.Request.Headers["roomToken"];
        if (string.IsNullOrWhiteSpace(roomToken))
            return default;

        // 获取声明
        var jwtService = httpContext.RequestServices.GetRequiredService<IJwtService>();
        var claims = jwtService.ReadToken(roomToken!);
        var roomId = claims.FirstOrDefault(x => x.Type == JwtConstants.Claims.RoomId)?.Value;
        if (string.IsNullOrWhiteSpace(roomId))
            return default;

        // 获取房间信息
        var roomService = httpContext.RequestServices.GetRequiredService<IRoomService>();
        var result = await roomService.GetAsync(roomId);
       return result.Match(res => res, ex => default);
    }
}