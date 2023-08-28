namespace Infrastructure.ValueObjects;

/// <summary>
///     在线用户信息
/// </summary>
public class UserInfo
{
    /// <summary>
    ///     用户编号
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    ///     用户昵称
    /// </summary>
    public string? NickName { get; set; } 

    /// <summary>
    ///     手机号码
    /// </summary>
    public string? PhoneNumber { get; set; } 

    /// <summary>
    ///     用户 Email
    /// </summary>
    public string? Email { get; set; } 

    /// <summary>
    ///     用户头像
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    ///     是否管理
    /// </summary>
    public bool IsAdmin { get; set; }

    /// <summary>
    ///     地址
    /// </summary>
    public Address? Address { get; set; }
}