using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Molo.Infrastructure.MessageBroker
{
    public abstract class MessageBrokerProducerBase<T> : MessageBrokerBase, IMessageBrokerProducer<T>
    {
        private readonly string _exchangeName;
        private readonly string _routingKey;
        private readonly string _appId;

        public MessageBrokerProducerBase(ConnectionFactory connectionFactory, string exchangeName, 
            string queueName, string routingKey, string appId) 
            : base(connectionFactory, exchangeName, queueName, routingKey) 
        {
            _exchangeName = exchangeName;
            _routingKey = routingKey;
            _appId = appId;
        }

        public void Publish(T @event)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize<T>(@event));
                var properties = Channel.CreateBasicProperties();
                properties.AppId = _appId;
                properties.ContentType = "application/json";
                properties.DeliveryMode = 1;
                properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                Channel.BasicPublish(exchange: _exchangeName, routingKey: _routingKey, body: body, basicProperties: properties);
            }
            catch (Exception ex)
            {
                //TODO: Log Event
            }
        }
    }
}
