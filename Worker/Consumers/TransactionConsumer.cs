using Molo.Application.Molo.Subscription.Commands;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Molo.Application.Molo.Transact.Commands;
using Molo.Worker.Workers;

namespace Molo.Worker.Consumers
{
    public class TransactionConsumer : ConsumerBase, IHostedService
    {
        private const string ExchangeName = "default";
        private const string QueueName = "transaction";
        private const string RoutingKey = "transaction";

        public TransactionConsumer(IServiceScopeFactory serviceScopeFactory, ConnectionFactory connectionFactory,
            ILogger<SubscriptionConsumer> logger) :
            base(serviceScopeFactory, connectionFactory, ExchangeName, QueueName, RoutingKey, logger)
        {

            try
            {
                var consumer = new AsyncEventingBasicConsumer(Channel);
                consumer.Received += OnEventReceived<TransactionEventCommand>;
                Channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, $"Error while consuming message. ExchangeName: {ExchangeName}; RoutingKey: {RoutingKey}; QueueName: {QueueName}");
            }
        }

        public virtual Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();
            return Task.CompletedTask;
        }
    }
}
