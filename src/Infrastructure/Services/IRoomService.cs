using Infrastructure.Dtos.Room;
using LanguageExt.Common;

namespace Infrastructure.Services;

/// <summary>
/// </summary>
public interface IRoomService
{
    /// <summary>
    ///     Create
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public ValueTask<Result<RoomInfo>> CreateAsync(CreateInputDto input);

    /// <summary>
    ///     加入房间
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public ValueTask<Result<bool>> JoinAsync(string roomId, string userId);

    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ValueTask<Result<RoomInfo?>> GetAsync(string id);
}