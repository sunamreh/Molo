using MediatR;
using Molo.Application.Common.Exceptions;
using Molo.Application.Common.Interfaces;
using Molo.Domain.Entities;
using System.Text.Json.Serialization;

namespace Molo.Application.Molo.Subscription.Commands
{
    public class SubscribeCommand : IRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("msisdn")]
        public string Msisdn { get; set; }

        [JsonPropertyName("pin")]
        public string Pin { get; set; }
    }
    public class SubscribeCommandHandler : IRequestHandler<SubscribeCommand>
    {
        private readonly ISubscribeService _subscribeService;
        private readonly ICollectionService _collectionService;

        public SubscribeCommandHandler(ISubscribeService subscribeService, ICollectionService collectionService)
        {
            _subscribeService = subscribeService;
            _collectionService = collectionService;
        }

        public async Task Handle(SubscribeCommand request, CancellationToken cancellationToken)
        {
            bool subscriberExists = await _subscribeService.CheckSubscriberExists(request.Msisdn);

            if (subscriberExists)
                throw new ValidationException("Subscriber already exists");

            var isValidAccountHolder = await _collectionService.ValidateAccountHolder(request.Msisdn);

            if (!isValidAccountHolder)
                throw new NotFoundException();

            await _subscribeService.Publish(request);
        }
    }
}
