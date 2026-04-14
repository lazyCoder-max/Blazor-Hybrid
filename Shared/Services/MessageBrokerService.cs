using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Shared.Helpers;
using Shared.Interfaces;
using Shared.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Config;

namespace Shared.Services
{
    public class MessageBrokerService(
        MessageBrokerOptions settings,
        ILogger<MessageBrokerService> logger) : IMessageBrokerService
    {
        private IConnection? connection;
        private IChannel? channel;

        private string? replyQueueName;
        private AsyncEventingBasicConsumer? replyConsumer;
        private string? replyConsumerTag;

        // correlationId -> waiting caller
        private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> pendingRequests = new();

        public IChannel? Channel => channel;

        public async Task CleanupAsync()
        {
            foreach (var kvp in pendingRequests)
            {
                kvp.Value.TrySetCanceled();
            }

            pendingRequests.Clear();

            if (channel?.IsOpen == true && !string.IsNullOrWhiteSpace(replyConsumerTag))
            {
                await channel.BasicCancelAsync(replyConsumerTag);
            }

            if (channel?.IsOpen == true)
            {
                await channel.CloseAsync();
            }

            if (connection?.IsOpen == true)
            {
                await connection.CloseAsync();
            }
        }

        public async Task<(AsyncEventingBasicConsumer?, string?)> SetupConnectionAsync(RequestType request)
        {
            try
            {
                if (connection?.IsOpen == true && channel?.IsOpen == true)
                {
                    string existingQueueName = $"{request}Queue";
                    var existingConsumer = new AsyncEventingBasicConsumer(channel);
                    return (existingConsumer, existingQueueName);
                }

                ConnectionFactory factory = new()
                {
                    HostName = settings.URL,
                    UserName = settings.Username,
                    Password = settings.Password,
                    VirtualHost = settings.VirtualHost,
                    ClientProvidedName = request.ToString()
                };

                connection = await factory.CreateConnectionAsync();
                channel = await connection.CreateChannelAsync();

                string exchangeName = $"{request}Exchange";
                string queueName = $"{request}Queue";
                string routingKey = $"{request.ToString().ToLowerInvariant()}-routing-key";

                await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Direct, durable: true);
                await channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                await channel.QueueBindAsync(queueName, exchangeName, routingKey, arguments: null);

                // Good for worker-style RPC servers
                await channel.BasicQosAsync(0, 100, false);

                var consumer = new AsyncEventingBasicConsumer(channel);
                return (consumer, queueName);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error setting up message broker connection");
                return (null, null);
            }
        }

        public async Task<string> StartConsumingAsync(AsyncEventingBasicConsumer consumer, string queueName)
        {
            return await channel!.BasicConsumeAsync(queueName, autoAck: false, consumer);
        }

        /// <summary>
        /// Creates one exclusive reply queue per client instance and starts a consumer on it.
        /// RabbitMQ recommends one callback queue per client and matching replies by CorrelationId.
        /// </summary>
        private async Task EnsureReplyConsumerAsync()
        {
            if (channel is null)
            {
                throw new InvalidOperationException("Channel is not initialized.");
            }

            if (replyConsumer is not null && !string.IsNullOrWhiteSpace(replyQueueName))
            {
                return;
            }

            replyQueueName = $"rpc.reply.{Guid.NewGuid():N}";

            await channel.QueueDeclareAsync(
                queue: replyQueueName,
                durable: false,
                exclusive: true,
                autoDelete: true,
                arguments: null);

            replyConsumer = new AsyncEventingBasicConsumer(channel);

            replyConsumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    string? correlationId = ea.BasicProperties.CorrelationId;

                    if (!string.IsNullOrWhiteSpace(correlationId) &&
                        pendingRequests.TryRemove(correlationId, out var tcs))
                    {
                        string response = Encoding.UTF8.GetString(ea.Body.ToArray());
                        tcs.TrySetResult(response);
                    }

                    // autoAck=true on reply queue, so nothing to ack here
                    await Task.CompletedTask;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing RPC reply");
                }
            };

            replyConsumerTag = await channel.BasicConsumeAsync(
                queue: replyQueueName,
                autoAck: true,
                consumer: replyConsumer);
        }

        /// <summary>
        /// RPC client method: send request and await typed response.
        /// </summary>
        public async Task<TResponse?> CallRpcAsync<TRequest, TResponse>(
            RequestType requestType,
            TRequest request,
            CancellationToken cancellationToken = default)
        {
            if (channel is null || connection is null || !channel.IsOpen || !connection.IsOpen)
            {
                var (_, _) = await SetupConnectionAsync(requestType);
            }

            if (channel is null)
            {
                throw new InvalidOperationException("RabbitMQ channel was not created.");
            }

            await EnsureReplyConsumerAsync();

            string correlationId = Guid.NewGuid().ToString("N");
            var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

            if (!pendingRequests.TryAdd(correlationId, tcs))
            {
                throw new InvalidOperationException("Failed to register pending RPC request.");
            }

            using var ctr = cancellationToken.Register(() =>
            {
                if (pendingRequests.TryRemove(correlationId, out var pending))
                {
                    pending.TrySetCanceled(cancellationToken);
                }
            });

            string exchangeName = $"{requestType}Exchange";
            string routingKey = $"{requestType.ToString().ToLowerInvariant()}-routing-key";

            var props = new BasicProperties
            {
                CorrelationId = correlationId,
                ReplyTo = replyQueueName,
                Persistent = true,
                ContentType = "application/json"
            };

            byte[] body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(request));

            await channel.BasicPublishAsync(
                exchange: exchangeName,
                routingKey: routingKey,
                mandatory: false,
                basicProperties: props,
                body: body,
                cancellationToken: cancellationToken);

            string rawResponse = await tcs.Task;

            return JsonSerializer.Deserialize<TResponse>(rawResponse);
        }

        public async Task<string?> StartRpcServerAsync<TRequest, TResponse>(
            RequestType requestType,
            Func<TRequest, Task<TResponse>> handler,
            CancellationToken cancellationToken = default)
        {
            var (consumer, queueName) = await SetupConnectionAsync(requestType);

            if (consumer is null || string.IsNullOrWhiteSpace(queueName) || channel is null)
            {
                return null;
            }

            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    TRequest? request = JsonSerializer.Deserialize<TRequest>(ea.Body.ToArray());

                    if (request is null)
                    {
                        logger.LogWarning("Received null or invalid RPC request for {RequestType}", requestType);
                        await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
                        return;
                    }

                    TResponse response = await handler(request);

                    byte[] responseBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));

                    var replyProps = new BasicProperties
                    {
                        CorrelationId = ea.BasicProperties.CorrelationId,
                        ContentType = "application/json",
                        Persistent = true
                    };

                    // Reply to the callback queue sent by the caller.
                    await channel.BasicPublishAsync(
                        exchange: string.Empty, // default exchange
                        routingKey: ea.BasicProperties.ReplyTo!,
                        mandatory: false,
                        basicProperties: replyProps,
                        body: responseBytes,
                        cancellationToken: cancellationToken);

                    await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error handling RPC request for {RequestType}", requestType);

                    // Requeue=true if you want retries; false if poison messages should be dropped/DLQ'd
                    await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
                }
            };

            return await StartConsumingAsync(consumer, queueName);
        }
    }
}