using Infrastructure.ValueObjects;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.SignalR;

/// <summary>
///     SignalR连接用户缓存
/// </summary>
public class ConnectionUserCache : IConnectionUserCache
{
    private readonly IDistributedCache _cache;

    /// <summary>
    ///     初始化一个<see cref="ConnectionUserCache" />类型的新实例
    /// </summary>
    public ConnectionUserCache(IDistributedCache cache)
    {
        _cache = cache;
    }

    /// <summary>
    ///     获取缓存的用户信息
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <returns></returns>
    public virtual async Task<ConnectionUser?> GetUserAsync(string userId)
    {
        var key = GetKey(userId);
        return await _cache.GetAsync<ConnectionUser>(key);
    }

    /// <summary>
    ///     获取指定用户的所有连接Id
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <returns></returns>
    public virtual async Task<string[]> GetConnectionIdsAsync(string userId)
    {
        var user = await GetUserAsync(userId);
        return user.ConnectionIds.ToArray();
    }

    /// <summary>
    ///     设置用户缓存
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="user">用户信息</param>
    /// <returns></returns>
    public virtual async Task SetUserAsync(string userId, ConnectionUser? user)
    {
        var key = GetKey(userId);
        await _cache.SetAsync(key, user);
    }

    /// <summary>
    ///     添加指定用户的连接
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="connectionId">连接Id</param>
    /// <returns></returns>
    public virtual async Task AddConnectionIdAsync(string userId, string connectionId)
    {
        var user = await GetUserAsync(userId);
        user.ConnectionIds.AddIfNotExist(connectionId);
        await SetUserAsync(userId, user);
    }

    /// <summary>
    ///     移除指定用户的连接
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="connectionId">连接Id</param>
    /// <returns></returns>
    public virtual async Task RemoveConnectionIdAsync(string userId, string connectionId)
    {
        var user = await GetUserAsync(userId);
        if (!user.ConnectionIds.Contains(connectionId)) return;
        user.ConnectionIds.Remove(connectionId);
        if (user.ConnectionIds.Count == 0)
        {
            await RemoveUserAsync(userId);
            return;
        }

        await SetUserAsync(userId, user);
    }

    /// <summary>
    ///     移除指定用户的缓存
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <returns></returns>
    public virtual async Task RemoveUserAsync(string userId)
    {
        var key = GetKey(userId);
        await _cache.RemoveAsync(key);
    }

    /// <summary>
    ///     统计总连接数
    /// </summary>
    public async Task IncrementConnectionsCount()
    {
        var currentMoth = DateTime.Now.ToString("yyyyMMdd");
        var key = $"signalR_connection_users_{currentMoth}";
        var value = await _cache.GetAsync<int>(key);
        var count = value == default ? 0 : value + 1;
        await _cache.SetAsync(key, count);
    }
    
    /// <summary>
    ///     统计总连接数
    /// </summary>
    public async Task DecrementalConnectionsCount()
    {
        var currentMoth = DateTime.Now.ToString("yyyyMMdd");
        var key = $"signalR_connection_users_{currentMoth}";
        var value = await _cache.GetAsync<int>(key);
        var count = value == default ? 0 : value - 1;
        await _cache.SetAsync(key, count < 0 ? 0 : count);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    private static string GetKey(string userId)
    {
        return $"signalR_connection_user_{userId}";
    }
}