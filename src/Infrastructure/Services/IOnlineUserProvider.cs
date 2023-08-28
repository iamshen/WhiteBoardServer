using Infrastructure.ValueObjects;

namespace Infrastructure.Services;

/// <summary>
///     定义在线用户提供者
/// </summary>
public interface IOnlineUserProvider
{
    /// <summary>
    ///     获取或创建在线用户信息
    /// </summary>
    /// <param name="userInfo">用户</param>
    /// <returns>在线用户信息</returns>
    Task<UserInfo?> GetOrCreate(UserInfo userInfo);

    /// <summary>
    ///     移除在线用户信息
    /// </summary>
    /// <param name="userIds">用户ids</param>
    void Remove(params string[] userIds);
}