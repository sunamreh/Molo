using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Molo.Application.Molo.Subscription.Commands;
using Molo.Worker.Consumers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using static System.Formats.Asn1.AsnWriter;

namespace Molo.Worker.Workers
{
    public class SubscriptionConsumer : ConsumerBase, IHostedService
    {
        private const string ExchangeName = "default";
        private const string QueueName = "subscribe";
        private const string RoutingKey = "subscribe";

        public SubscriptionConsumer(IServiceScopeFactory serviceScopeFactory, ConnectionFactory connectionFactory, 
            ILogger<SubscriptionConsumer> logger) : 
            base(serviceScopeFactory, connectionFactory, ExchangeName, QueueName, RoutingKey, logger)
        {

            try
            {
                var consumer = new AsyncEventingBasicConsumer(Channel);
                consumer.Received += OnEventReceived<SubscribeEventCommand>;
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
