using Molo.Application.Molo.Subscription.Commands;
using Molo.Domain.Entities;
using RabbitMQ.Client;

namespace Molo.Infrastructure.MessageBroker
{
    public class SubscriptionProducer : MessageBrokerProducerBase<SubscribeCommand>
    {
        private const string ExchangeName = "default";
        private const string QueueName = "subscribe";
        private const string RoutingKey = "subscribe";

        public SubscriptionProducer(ConnectionFactory connectionFactory) : 
            base(connectionFactory, ExchangeName, QueueName, RoutingKey, nameof(SubscriptionProducer)) { }
    }
}
