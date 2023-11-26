using MediatR;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Molo.Worker.Consumers
{
    public class ConsumerBase : RabbitMqClientBase
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger _logger;

        public ConsumerBase(
            IServiceScopeFactory serviceScopeFactory,
            ConnectionFactory connectionFactory, string exchangeName,
            string queueName, string routingKey, ILogger logger) :
            base(connectionFactory, exchangeName, queueName, routingKey)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected virtual async Task OnEventReceived<T>(object sender, BasicDeliverEventArgs @event)
        {
            try
            {
                var body = Encoding.UTF8.GetString(@event.Body.ToArray());
                var message = JsonSerializer.Deserialize<T>(body);

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    await mediator.Send(message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Error while retrieving message from queue.");
            }
            finally
            {
                Channel.BasicAck(@event.DeliveryTag, false);
            }
        }
    }
}
