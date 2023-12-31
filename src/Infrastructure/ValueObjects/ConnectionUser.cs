﻿namespace Infrastructure.ValueObjects;

/// <summary>
///     SignalR 连接用户项
/// </summary>
public class ConnectionUser
{
    /// <summary>
    ///     获取或设置 用户名
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    ///     获取或设置 连接Id集合
    /// </summary>
    public ICollection<string> ConnectionIds { get; set; } = new List<string>();
}