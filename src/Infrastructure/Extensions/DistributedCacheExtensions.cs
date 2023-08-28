using System.Text.Json;
using Ardalis.GuardClauses;

namespace Microsoft.Extensions.Caching.Distributed;

/// <summary>
///     <see cref="IDistributedCache" />扩展方法
/// </summary>
public static class DistributedCacheExtensions
{
    /// <summary>
    ///     将对象存入缓存中
    /// </summary>
    public static void Set(this IDistributedCache cache, string key, object? value,
        DistributedCacheEntryOptions? options = null)
    {
        Guard.Against.NullOrEmpty(key, nameof(key));
        Guard.Against.Null(value, nameof(value));

        var json = value.Serialize();
        if (options == null)
            cache.SetString(key, json);
        else
            cache.SetString(key, json, options);
    }

    /// <summary>
    ///     异步将对象存入缓存中
    /// </summary>
    public static async Task SetAsync(this IDistributedCache cache, string key, object? value,
        DistributedCacheEntryOptions? options = null)
    {
        Guard.Against.NullOrEmpty(key, nameof(key));
        Guard.Against.Null(value, nameof(value));

        var json = value.Serialize();
        if (options == null)
            await cache.SetStringAsync(key, json);
        else
            await cache.SetStringAsync(key, json, options);
    }

    /// <summary>
    ///     将对象存入缓存中，使用指定时长
    /// </summary>
    public static void Set(this IDistributedCache cache, string key, object? value, int cacheSeconds)
    {
        Guard.Against.NullOrEmpty(key, nameof(key));
        Guard.Against.Null(value, nameof(value));
        Guard.Against.Zero(cacheSeconds, nameof(cacheSeconds));

        var options = new DistributedCacheEntryOptions();
        options.SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheSeconds));
        cache.Set(key, value, options);
    }

    /// <summary>
    ///     异步将对象存入缓存中，使用指定时长
    /// </summary>
    public static Task SetAsync(this IDistributedCache cache, string key, object? value, int cacheSeconds)
    {
        Guard.Against.NullOrEmpty(key, nameof(key));
        Guard.Against.Null(value, nameof(value));
        Guard.Against.Zero(cacheSeconds, nameof(cacheSeconds));

        var options = new DistributedCacheEntryOptions();
        options.SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheSeconds));
        return cache.SetAsync(key, value, options);
    }

    /// <summary>
    ///     获取指定键的缓存项
    /// </summary>
    public static TResult? Get<TResult>(this IDistributedCache cache, string key)
    {
        var json = cache.GetString(key);
        if (json == null) return default;
        return json.Deserialize<TResult>();
    }

    /// <summary>
    ///     异步获取指定键的缓存项
    /// </summary>
    public static async Task<TResult?> GetAsync<TResult>(this IDistributedCache cache, string key)
    {
        var json = await cache.GetStringAsync(key);
        return json == null ? default : json.Deserialize<TResult>();
    }

    /// <summary>
    ///     获取指定键的缓存项，不存在则从指定委托获取，并回存到缓存中再返回
    /// </summary>
    public static TResult? Get<TResult>(this IDistributedCache cache, string key, Func<TResult> getFunc,
        DistributedCacheEntryOptions? options = null)
    {
        var result = cache.Get<TResult>(key);
        if (!Equals(result, default(TResult))) return result;
        result = getFunc();
        if (Equals(result, default(TResult))) return default;
        cache.Set(key, result, options);
        return result;
    }

    /// <summary>
    ///     异步获取指定键的缓存项，不存在则从指定委托获取，并回存到缓存中再返回
    /// </summary>
    public static async Task<TResult?> GetAsync<TResult>(this IDistributedCache cache, string key,
        Func<Task<TResult>> getAsyncFunc, DistributedCacheEntryOptions? options = null)
    {
        var result = await cache.GetAsync<TResult>(key);
        if (!Equals(result, default(TResult))) return result;
        result = await getAsyncFunc();
        if (Equals(result, default(TResult))) return default;
        await cache.SetAsync(key, result, options);
        return result;
    }

    /// <summary>
    ///     获取指定键的缓存项，不存在则从指定委托获取，并回存到缓存中再返回
    /// </summary>
    public static TResult? Get<TResult>(this IDistributedCache cache, string key, Func<TResult> getFunc,
        int cacheSeconds)
    {
        Guard.Against.Zero(cacheSeconds, nameof(cacheSeconds));

        var options = new DistributedCacheEntryOptions();
        options.SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheSeconds));
        return cache.Get(key, getFunc, options);
    }

    /// <summary>
    ///     异步获取指定键的缓存项，不存在则从指定委托获取，并回存到缓存中再返回
    /// </summary>
    public static Task<TResult?> GetAsync<TResult>(this IDistributedCache cache, string key,
        Func<Task<TResult>> getAsyncFunc, int cacheSeconds)
    {
        Guard.Against.Zero(cacheSeconds, nameof(cacheSeconds));

        var options = new DistributedCacheEntryOptions();
        options.SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheSeconds));
        return cache.GetAsync(key, getAsyncFunc, options);
    }

    /// <summary>
    ///     移除指定键的缓存项
    /// </summary>
    public static void Remove(this IDistributedCache cache, params string[] keys)
    {
        Guard.Against.Null(keys, nameof(keys));
        foreach (var key in keys) cache.Remove(key);
    }

    /// <summary>
    ///     移除指定键的缓存项
    /// </summary>
    public static async Task RemoveAsync(this IDistributedCache cache, params string[] keys)
    {
        Guard.Against.Null(keys, nameof(keys));
        foreach (var key in keys) await cache.RemoveAsync(key);
    }
}