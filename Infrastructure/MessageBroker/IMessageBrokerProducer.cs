namespace Molo.Infrastructure.MessageBroker
{
    public interface IMessageBrokerProducer<in T>
    {
        void Publish(T @event);
    }
}
