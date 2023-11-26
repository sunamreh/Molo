using MediatR;
using Molo.Application.Common.Interfaces;
using Molo.Domain.Entities;
using System.Text.Json.Serialization;

namespace Molo.Application.Molo.Subscription.Commands
{
    public class SubscribeEventCommand : IRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("msisdn")]
        public string Msisdn { get; set; }

        [JsonPropertyName("pin")]
        public string Pin { get; set; }
    }

    public class SubscribeEventCommandHandler : IRequestHandler<SubscribeEventCommand>
    {
        private readonly ISubscribeService _subscribeService;

        public SubscribeEventCommandHandler(ISubscribeService subscribeService)
        {
            _subscribeService = subscribeService;
        }

        public async Task Handle(SubscribeEventCommand request, CancellationToken cancellationToken)
        {
            var subscriber = new Subscriber
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Msisdn = request.Msisdn,
                Pin = request.Pin,
                SubscriptionDate = DateTimeOffset.Now,
                IsActive = true
            };

            await _subscribeService.Subscribe(subscriber);
        }
    }
}
