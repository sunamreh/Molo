using RabbitMQ.Client;

namespace Molo.Infrastructure.MessageBroker
{
    public abstract class MessageBrokerBase : IDisposable
    {
        private readonly string _exchangeName;
        private readonly string _queueName;
        private readonly string _routingKey;

        protected IModel Channel { get; private set; }
        private IConnection _connection;
        private readonly ConnectionFactory _connectionFactory;

        protected MessageBrokerBase(
            ConnectionFactory connectionFactory, string exchangeName, string queueName, string routingKey)
        {
            _connectionFactory = connectionFactory;
            _exchangeName = exchangeName;
            _queueName = queueName;
            _routingKey = routingKey;
            ConnectToRabbitMq();
        }

        private void ConnectToRabbitMq()
        {
            if (_connection == null || _connection.IsOpen == false)
            {
                _connection = _connectionFactory.CreateConnection();
            }

            if (Channel == null || Channel.IsOpen == false)
            {
                Channel = _connection.CreateModel();
                Channel.ExchangeDeclare(
                    exchange: _exchangeName,
                    type: "direct",
                    durable: true,
                    autoDelete: false);

                Channel.QueueDeclare(
                    queue: _queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false);

                Channel.QueueBind(
                    queue: _queueName,
                    exchange: _exchangeName,
                    routingKey: _routingKey);
            }
        }

        public void Dispose()
        {
            Channel?.Close();
            Channel?.Dispose();
            Channel = null;

            _connection?.Close();
            _connection?.Dispose();
            _connection = null;
        }
    }
}
