using MediatR;
using Molo.Application.Common.Interfaces;
using Molo.Domain.Entities;

namespace Molo.Application.Molo.Subscription.Queries
{
    public class SubscriberExistsQuery : IRequest<bool>
    {
        public string Msisdn { get; set; }
    }

    public class SubscriberExistsQueryHandler : IRequestHandler<SubscriberExistsQuery, bool>
    {
        private readonly IMoloDbRepository<Subscriber> _repository;

        public SubscriberExistsQueryHandler(IMoloDbRepository<Subscriber> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(SubscriberExistsQuery request, CancellationToken cancellationToken)
        {
            var subscriber = await _repository.Get(s => s.Msisdn == request.Msisdn);

            return subscriber != null;
        }
    }
}
