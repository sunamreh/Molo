using Molo.Application.Molo.Transact.Commands;
using RabbitMQ.Client;

namespace Molo.Infrastructure.MessageBroker
{
    public class TransactionProducer : MessageBrokerProducerBase<CreateTransactionCommand>
    {
        private const string ExchangeName = "default";
        private const string QueueName = "transaction";
        private const string RoutingKey = "transaction";

        public TransactionProducer(ConnectionFactory connectionFactory) :
            base(connectionFactory, ExchangeName, QueueName, RoutingKey, nameof(TransactionProducer))
        { }
    }
}
