namespace Infrastructure.Dtos.Room;

/// <summary>
/// </summary>
public class CreateInputDto
{
    /// <summary>
    ///     是否开启录制：true：开启。 false: 不开启。字段默认值为 true。
    /// </summary>
    public bool IsRecord { get; set; }

    /// <summary>
    ///     房间内可写人数（拥有 writer 或 admin 权限的用户）的上限。如果传 0，则表示无限制。目前推荐设置为 0。
    /// </summary>
    public int Limit { get; set; }
}