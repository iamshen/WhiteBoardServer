using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMqConsoleApp;

namespace KingMetal.MessageBus.RabbitMQ.MessageBus;

public class RabbitMQConsumer : IMessageConsumer, IDisposable
{
    private readonly ILogger logger;
    private readonly SemaphoreSlim semaphoreSlim = new(1);
    private Func<IMessageConsumer, IList<MessageDeliveryObject>, Task> consumerHandler = null!;
    private bool isDisposed; // 更改 isDispose 为 isDisposed
    private IOptionsMonitor<RabbitMQOptions> rabbitMQOptions;

    private RabbitMQConsumer(IOptionsMonitor<RabbitMQOptions> rabbitMQOptions,
        string queue,
        string routingKey,
        IModel channel,
        ILogger<RabbitMQConsumer> logger,
        Func<IMessageConsumer, IList<MessageDeliveryObject>, Task> consumerHandler)
    {
        if (consumerHandler is null)
            throw new ArgumentNullException(nameof(consumerHandler), $"{queue}队列消息处理器为NULL");

        this.consumerHandler = consumerHandler;
        this.rabbitMQOptions = rabbitMQOptions;
        RabbitMQChannel = channel;
        this.logger = logger;
        QueueName = queue;
        RoutingKey = routingKey;
        BindQueue(channel, queue, routingKey);
    }

    private IModel RabbitMQChannel { get; }
    public string QueueName { get; init; }
    public string RoutingKey { get; init; }

    // ... (保留原有代码)

    public void Dispose()
    {
        if (!isDisposed) // 将 !isDispose 改为 !isDisposed
        {
            isDisposed = true; // 将 isDispose = true 改为 isDisposed = true
            RabbitMQChannel.Dispose();
        }
    }

    public static async Task<RabbitMQConsumer> Create(IOptionsMonitor<RabbitMQOptions> rabbitMQOptions,
        string queue,
        string routingKey,
        IModel channel,
        ILogger<RabbitMQConsumer> logger,
        Func<IMessageConsumer, IList<MessageDeliveryObject>, Task> consumerHandler)
    {
        var consumer = new RabbitMQConsumer(rabbitMQOptions, queue, routingKey, channel, logger, consumerHandler);
        await consumer.StartConsumingAsync(); // 添加此行以在创建后立即开始消费消息
        return consumer;
    }

    public void Ack(ulong deliveryTag, bool multiple)
    {
        try
        {
            RabbitMQChannel.BasicAck(deliveryTag, multiple);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{QueueName}队列消息{deliveryTag}确认失败");
        }
    }

    public void Nack(ulong deliveryTag, bool multiple, bool enqueue)
    {
        try
        {
            RabbitMQChannel.BasicNack(deliveryTag, multiple, enqueue);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{QueueName}队列消息{deliveryTag}拒绝失败");
        }
    }

    private void BindQueue(IModel channel, string queue, string routingKey)
    {
        // ... (保留原有代码)
    }

    private async Task StartConsumingAsync() // 新增此方法以开始消费消息
    {
        await Task.Run(() => ConsumeMessages());
    }

    private async Task ConsumeMessages()
    {
        // 消费消息的逻辑
        // ... (保留原有代码)
    }
}

internal class MessageDeliveryObject
{
}

public interface IMessageConsumer
{
}