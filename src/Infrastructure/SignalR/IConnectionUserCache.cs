using Infrastructure.ValueObjects;

namespace Infrastructure.SignalR;

/// <summary>
/// 定义SignalR连接用户缓存
/// </summary>
public interface IConnectionUserCache
{
    /// <summary>
    /// 获取缓存的用户信息
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <returns></returns>
    Task<ConnectionUser?> GetUserAsync(string userId);

    /// <summary>
    /// 获取指定用户的所有连接Id
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <returns></returns>
    Task<string[]> GetConnectionIdsAsync(string userId);

    /// <summary>
    /// 设置用户缓存
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="user">用户信息</param>
    /// <returns></returns>
    Task SetUserAsync(string userId, ConnectionUser? user);

    /// <summary>
    /// 添加指定用户的连接
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="connectionId">连接Id</param>
    /// <returns></returns>
    Task AddConnectionIdAsync(string userId, string connectionId);

    /// <summary>
    /// 移除指定用户的连接
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="connectionId">连接Id</param>
    /// <returns></returns>
    Task RemoveConnectionIdAsync(string userId, string connectionId);

    /// <summary>
    /// 移除指定用户的缓存
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <returns></returns>
    Task RemoveUserAsync(string userId);

    /// <summary>
    /// 统计连接数
    /// </summary>
    /// <returns></returns>
    Task IncrementConnectionsCount();
    
    /// <summary>
    /// 统计连接数
    /// </summary>
    /// <returns></returns>
    Task DecrementalConnectionsCount();
}