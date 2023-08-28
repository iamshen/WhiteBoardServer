namespace Infrastructure.Dtos.Room;

/// <summary>
/// </summary>
public class RoomInfo
{
    /// <summary>
    ///     房间的 ID，即房间的全局唯一标识符。
    /// </summary>
    public string RoomId { get; set; } = string.Empty;

    /// <summary>
    ///     创建人Id
    /// </summary>
    public string? CreatorId { get; set; } = string.Empty;

    /// <summary>
    ///     房间是否开启录制：true：开启。false: 不开启。
    /// </summary>
    public bool IsRecord { get; set; }

    /// <summary>
    ///     房间是否被封禁：true：已封禁。 false: 未封禁。
    /// </summary>
    public bool IsForbidden { get; set; }

    /// <summary>
    ///     创建房间的 UTC 时间。
    ///     <example>2023-08-24T17:56:29.129Z</example>
    /// </summary>
    public string CreatedTime { get; set; } = string.Empty;

    /// <summary>
    ///     房间内可写人数（拥有 writer 或 admin 权限的用户）的上限。如果值为 0，则表示无限制。
    /// </summary>
    public int Limit { get; set; }

    /// <summary>
    ///     房间用户集合
    /// </summary>
    public List<string>? UserIds { get; set; }

    /// <summary>
    ///     被踢出的用户集合
    /// </summary>
    public List<string>? BlackUserIds { get; set; }
}