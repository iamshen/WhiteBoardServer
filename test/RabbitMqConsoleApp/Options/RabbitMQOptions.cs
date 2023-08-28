using RabbitMQ.Client;

namespace RabbitMqConsoleApp;

/// <summary>
///     RabbitMQ配置
/// </summary>
public class RabbitMQOptions
{
    /// <summary>
    ///     交换机名称
    /// </summary>
    public string ExchangeName { get; set; } = "KingMetal_Exchange_Test";

    /// <summary>
    ///     服务器多个HostName之间用逗号隔开
    /// </summary>
    public string HostNames { get; set; } = null!;

    /// <summary>
    ///     服务器端点集合
    /// </summary>
    internal List<AmqpTcpEndpoint> EndPoints => HostNames.Split(',').Select(m => AmqpTcpEndpoint.Parse(m)).ToList();

    /// <summary>
    ///     用户名
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    ///     密码
    /// </summary>
    public string Password { get; set; } = null!;

    /// <summary>
    ///     VirtualHost
    /// </summary>
    public string VirtualHost { get; set; } = "/";

    /// <summary>
    ///     交换机类型
    /// </summary>
    public string ExchangeType { get; set; } = "topic";

    /// <summary>
    ///     最大批量发送消息的数量
    /// </summary>
    public int MaxBatchPushCount { get; set; } = 2000;

    /// <summary>
    ///     当消息未确认查过该值时将不在推送消息
    /// </summary>
    public int MaxPrefetchCount { get; set; } = 10000;

    /// <summary>
    ///     消费批处理大小
    /// </summary>
    public int ConsumeBatchSize { get; set; } = 5000;

    /// <summary>
    ///     拉取消息都休眠时间(MS)，也就是说如果消息队列中没有消息都时候就休眠这么多事件后再轮询拉取
    /// </summary>
    public int PullMessageDelay { get; set; } = 100;
}