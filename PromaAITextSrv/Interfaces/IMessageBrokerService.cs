using PromaAITextSrv.Helpers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PromaAITextSrv.Interfaces
{
    public interface IMessageBrokerService
    {
        IChannel? Channel { get; }

        Task CleanupAsync();
        Task<(AsyncEventingBasicConsumer?, string?)> SetupConnectionAsync(RequestType request);
        Task<string> StartConsumingAsync(AsyncEventingBasicConsumer consumer, string queueName);
        Task<TResponse?> CallRpcAsync<TRequest, TResponse>(RequestType requestType, TRequest request, CancellationToken cancellationToken = default);
        Task<string?> StartRpcServerAsync<TRequest, TResponse>(RequestType requestType, Func<TRequest, Task<TResponse>> handler, CancellationToken cancellationToken = default);
    }
}