using System.Security.Claims;
using Infrastructure.Constants;
using Infrastructure.Dtos.Room;
using Infrastructure.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebSocketServer.Hubs;

// 加入互动白板房间的基本流程
// 1. 客户端 使用 http 发起创建房间的请求
// 2. 成功创建房间后，服务端返回新建房间的 roomId roomToken
// 3. 客户端调用 Signalr SDK 加入白板房间。
[Authorize]
public class DrawHub : Hub
{
    private const string DefaultGroup = "defaultGroup";
    private readonly IConnectionUserCache _cache;
    private readonly ILogger _logger;

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="factory"></param>
    public DrawHub(IConnectionUserCache cache, ILoggerFactory factory)
    {
        _cache = cache;
        _logger = factory.CreateLogger<DrawHub>();
    }

    /// <summary>
    ///     canvas 绘画
    /// </summary>
    /// <param name="prevX"></param>
    /// <param name="prevY"></param>
    /// <param name="currentX"></param>
    /// <param name="currentY"></param>
    /// <param name="color"></param>
    public virtual async Task Draw(int prevX, int prevY, int currentX, int currentY, string color)
    {
        // 1. 谁操作-画笔操作人
        var user = Context.User.GetUserInfo();
        if (user is null)
        {
            await Clients.Caller.SendAsync(Events.DrawError, Errors.UnAuthentication);
            return;
        }

        var room = await Context.GetRoomAsync();
        if (room is null)
        {
            await Clients.Caller.SendAsync(Events.DrawError, Errors.RoomNotFound);
            return;
        }
        
        // 2. 确保操作人拥有绘画权限
        if (!EnsureDrawPermissions(room))
        {
            await Clients.Caller.SendAsync(Events.DrawError, Errors.UnAuthority);
            return;
        }
        
        // 3. 发送给谁- 广播给当前操作人的房间的 group
        await Clients.GroupExcept(room.RoomId, Context.ConnectionId).SendAsync(HubClientMethods.Draw, prevX, prevY, currentX, currentY, color, user);

        // 4. 把绘图数据写入 分布式缓存 redis 中, 以提供后续可以执行撤销操作.
        
        
        
    }

    private static bool EnsureDrawPermissions(RoomInfo roomInfo)
    {
        //TODO: 确保操作人拥有绘画权限

        return true;
    }

    /// <summary>
    /// 加入房间
    /// </summary>
    /// <returns></returns>
    public virtual async Task JoinRoom()
    {  
        var roomInfo = await Context.GetRoomAsync();
        if (roomInfo is null) return;

        await Groups.AddToGroupAsync(Context.ConnectionId, roomInfo.RoomId);
    }

    /// <summary>
    /// 离开房间
    /// </summary>
    /// <param name="roomIds">房间Id</param>
    /// <returns></returns>
    public virtual async Task RemoveFromRoom(IEnumerable<string> roomIds)
    {
        var roomInfo = await Context.GetRoomAsync();
        if (roomInfo is null) return;

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomInfo.RoomId);
    }

    #region 连接已打开

    /// <summary>
    ///     连接已打开
    /// </summary>
    /// <returns>表示异步连接的 <see cref="T:System.Threading.Tasks.Task" /></returns>
    /// <remarks>
    ///     <para> 权限验证： 进行身份验证或权限验证。</para>
    ///     <para> 加入默认组： 可以调用 Groups.AddToGroupAsync，将所有连接的用户加入某个默认的组</para>
    ///     <para> 上线通知： 可以发送广播消息，通知其他用户有新用户上线。</para>
    ///     <para> 统计连接数： 统计在线用户数量，维护一个连接计数器。</para>
    ///     <para> 连接日志： 记录用户的连接日志。</para>
    ///     <para> 跟踪活动用户： 跟踪活动用户，记录连接的用户信息。</para>
    ///     <para> 准备数据：  在一些场景中，可能需要在连接建立时准备一些数据，供后续的通信使用。</para>
    /// </remarks>
    public override async Task OnConnectedAsync()
    {
        // 身份验证
        var userId = Context.UserIdentifier;
        if (string.IsNullOrWhiteSpace(userId))
        {
            Context.Abort();
            await Clients.Caller.SendAsync(Events.ConnectionError, Errors.UnAuthentication);
            return;
        }

        // 权限验证
        if (!IsAuthorized())
        {
            Context.Abort();
            await Clients.Caller.SendAsync(Events.ConnectionError, Errors.UnAuthority);
            return;
        }

        var connectId = Context.ConnectionId;
        // 加入默认组
        await Groups.AddToGroupAsync(connectId, DefaultGroup);
        // 用户上线通知
        // 统计连接数
        await _cache.IncrementConnectionsCount();
        // 连接日志
        _logger.LogInformation(LogEvents.Connected, "连接已建立, ConnectId: {ConnectId}, UserId: {UserId}", connectId,
            userId);
        // 跟踪活动用户
        if (!string.IsNullOrEmpty(userId)) await _cache.AddConnectionIdAsync(userId, connectId);
        // 准备数据
        // ...

        // base on Connected
        await base.OnConnectedAsync();
    }

    #endregion

    #region 链接已断开

    /// <summary>
    ///     链接已断开
    /// </summary>
    /// <param name="exception"></param>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;

        if (!string.IsNullOrEmpty(userId)) await _cache.RemoveConnectionIdAsync(userId, Context.ConnectionId);

        await _cache.DecrementalConnectionsCount();
    }

    #endregion

    #region 私有方法

    /// <summary>
    ///     权限验证
    /// </summary>
    /// <returns></returns>
    private bool IsAuthorized()
    {
        // 验证账号状态
        var identity = Context.User?.Identity;
        var statusClaim = identity is not ClaimsIdentity claimsIdentity
            ? "0"
            : claimsIdentity.FindFirst(JwtConstants.Claims.Status)?.Value;

        var status = string.IsNullOrWhiteSpace(statusClaim) ? 0 : int.Parse(statusClaim);

        return status == 0;
    }

    #endregion

    public static class Errors
    {
        public const string UnAuthentication = "用户未登录!";
        public const string UnAuthority = "权限不足!";
        public const string RoomNotFound = "房间不存在";
    }

    public static class HubClientMethods
    {
        public const string Draw = "draw";
    }

    public static class LogEvents
    {
        public static EventId Connected => new(1);
    }

    public static class Events
    {
        public const string ConnectionError = "connectionError";
        public const string DrawError = "drawError";
    }
}