using Molo.Application.Molo.Collection.Command;
using Molo.Application.Molo.Transact.Commands;
using RabbitMQ.Client;

namespace Molo.Infrastructure.MessageBroker
{
    public class CollectProducer : MessageBrokerProducerBase<CollectCommand>
    {
        private const string ExchangeName = "default";
        private const string QueueName = "collect";
        private const string RoutingKey = "collect";

        public CollectProducer(ConnectionFactory connectionFactory) :
            base(connectionFactory, ExchangeName, QueueName, RoutingKey, nameof(CollectProducer))
        { }
    }
}
