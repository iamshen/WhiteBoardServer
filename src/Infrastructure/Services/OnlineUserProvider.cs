using Ardalis.GuardClauses;
using Infrastructure.Thread;
using Infrastructure.ValueObjects;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services;

/// <summary>
///     在线用户信息提供者
/// </summary>
public sealed class OnlineUserProvider : IOnlineUserProvider
{
    private readonly AsyncLock _asyncLock = new();
    private readonly IDistributedCache _cache;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    ///     初始化一个<see cref="T: Infrastructure.Services.OnlineUserProvider" />类型的新实例
    /// </summary>
    public OnlineUserProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _cache = serviceProvider.GetRequiredService<IDistributedCache>();
    }

    /// <summary>
    ///     获取或创建   在线用户信息
    /// </summary>
    /// <param name="userInfo">用户</param>
    /// <returns></returns>
    public async Task<UserInfo?> GetOrCreate(UserInfo userInfo)
    {
        var key = GetKey(userInfo.Id);
        using (await _asyncLock.LockAsync())
        {
            return await _cache.GetAsync(key, () => Task.FromResult(userInfo),
                new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(30) });
        }
    }

    /// <summary>
    ///     移除在线用户信息
    /// </summary>
    /// <param name="userIds">用户ids</param>
    public void Remove(params string[] userIds)
    {
        Guard.Against.Null(userIds, nameof(userIds));

        foreach (var userName in userIds)
        {
            var key = GetKey(userName);

            _cache.Remove(key);
        }
    }

    private static string GetKey(string userId)
    {
        return $"online_user_{userId}";
    }
}