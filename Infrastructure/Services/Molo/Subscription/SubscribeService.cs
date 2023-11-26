using Molo.Application.Common.Interfaces;
using Molo.Application.Molo.Subscription.Commands;
using Molo.Domain.Entities;
using Molo.Infrastructure.MessageBroker;

namespace Molo.Infrastructure.Services.Molo.Subscription
{
    public class SubscribeService : ISubscribeService
    {
        private readonly IMoloDbRepository<Subscriber> _moloDbRepository;
        private readonly IMessageBrokerProducer<SubscribeCommand> _producer;
        public SubscribeService(IMoloDbRepository<Subscriber> moloDbRepository, IMessageBrokerProducer<SubscribeCommand> producer)
        {
            _moloDbRepository = moloDbRepository;
            _producer = producer;
        }

        public async Task<bool> CheckSubscriberExists(string msisdn)
        {
            var subscriber = await _moloDbRepository.Get(s => s.Msisdn == msisdn);
            return subscriber != null;
        }

        public async Task Publish(SubscribeCommand subscriber)
        {
            _producer.Publish(subscriber);

            await Task.CompletedTask;
        }

        public async Task Subscribe(Subscriber subscriber)
        {
            await _moloDbRepository.Add(subscriber);
        }
    }
}
