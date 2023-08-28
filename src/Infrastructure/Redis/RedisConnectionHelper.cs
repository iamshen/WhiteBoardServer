using StackExchange.Redis;

namespace Infrastructure.Redis;

/// <summary>
///     Redis连接帮助类
/// </summary>
public class RedisConnectionHelper
{
    private static readonly IDictionary<string, ConnectionMultiplexer> ConnectionCache =
        new Dictionary<string, ConnectionMultiplexer>();

    // 使用信号量同步机制来保证数据一致性，多线程情况下只允许一个线程操作
    private static readonly SemaphoreSlim SemaphoreSlim = new(1, 1);

    /// <summary>
    ///     连接到指定服务器
    /// </summary>
    public static ConnectionMultiplexer Connect(string host)
    {
        SemaphoreSlim.Wait();
        try
        {
            if (ConnectionCache.TryGetValue(host, out var connection) && connection.IsConnected) return connection;

            connection = ConnectionMultiplexer.Connect(host);
            ConnectionCache[host] = connection;
            return connection;
        }
        finally
        {
            // 释放
            SemaphoreSlim.Release();
        }
    }
}