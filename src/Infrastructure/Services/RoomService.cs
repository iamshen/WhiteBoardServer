using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Infrastructure.Dtos.Room;
using LanguageExt.Common;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

/// <summary> Room Service </summary>
public class RoomService : IRoomService
{
    private const string RoomCacheKey = "_room_{0}";
    private readonly IDistributedCache _cache;
    private readonly ILogger<RoomService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public RoomService(IDistributedCache cache, IServiceProvider serviceProvider, ILogger<RoomService> logger)
    {
        _cache = cache;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async ValueTask<Result<RoomInfo>> CreateAsync(CreateInputDto input)
    {
        try
        {
            var id = Guid.NewGuid().ToString("D");
            var key = string.Format(RoomCacheKey, id);
            var creator = await GetUserId();
            var value = new RoomInfo
            {
                RoomId = id,
                CreatorId = creator,
                IsRecord = input.IsRecord,
                IsForbidden = false,
                CreatedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Limit = input.Limit,
                UserIds = new List<string>()
            };
            await _cache.SetAsync(key, value);

            return new Result<RoomInfo>(value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(CreateAsync) + ": {Input}", input.Serialize());
            var vex = new ValidationException(ex.Message);
            return new Result<RoomInfo>(vex);
        }
    }

    public async ValueTask<Result<bool>> JoinAsync(string roomId, string userId)
    {
        if (string.IsNullOrWhiteSpace(roomId))
        {
            var ex = new ValidationException(Errors.InValidRoomId);
            return new Result<bool>(ex);
        }

        if (string.IsNullOrWhiteSpace(userId))
        {
            var ex = new ValidationException(Errors.InValidUserId);
            return new Result<bool>(ex);
        }

        var key = string.Format(RoomCacheKey, roomId);
        var room = await _cache.GetAsync<RoomInfo>(key);
        if (room is null)
        {
            var ex = new ValidationException(Errors.NotFoundRoom);
            return new Result<bool>(ex);
        }

        room.UserIds!.Add(userId);

        return new Result<bool>(true);
    }

    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async ValueTask<Result<RoomInfo?>> GetAsync(string id)
    {
        var key = string.Format(RoomCacheKey, id);
        var room = await _cache.GetAsync<RoomInfo>(key);
        return new Result<RoomInfo?>(room);
    }

    #region 私有方法

    /// <summary>
    ///     获取当前登录用户
    /// </summary>
    /// <returns></returns>
    private async Task<string?> GetUserId()
    {
        return await _serviceProvider.GetRequiredService<IResolveService>().GetCurrentUserId();
    }

    #endregion


    public static class Errors
    {
        public const string InValidRoomId = "无效的房间Id";
        public const string InValidUserId = "无效的用户Id";
        public const string NotFoundRoom = "找不到指定的房间";
    }
}