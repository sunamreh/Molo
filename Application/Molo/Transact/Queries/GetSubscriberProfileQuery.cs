using MediatR;
using Molo.Application.Common.Interfaces;
using Molo.Domain.Entities;

namespace Molo.Application.Molo.Transact.Queries
{
    public class GetSubscriberProfileQuery : IRequest<SubscriberProfileDto>
    {
        public string Pin { get; set; }
        public string Msisdn { get; set; }
    }

    public class GetSubscriberProfileHandler : IRequestHandler<GetSubscriberProfileQuery, SubscriberProfileDto>
    {
        private readonly ITransactService _transactService;
        private readonly ICollectionService _getAccountBalanceService;

        public GetSubscriberProfileHandler(ITransactService transactService, ICollectionService getAccountBalanceService)
        {
            _transactService = transactService;
            _getAccountBalanceService = getAccountBalanceService;
        }

        public async Task<SubscriberProfileDto> Handle(GetSubscriberProfileQuery request, CancellationToken cancellationToken)
        {
            var accountBalance = await _getAccountBalanceService.GetAccountBalance();

            var subscriber = await _transactService.GetSubscriber(request.Msisdn);

            //TODO: Use Automapper/TinyMapper
            return new SubscriberProfileDto
            {
                SubscriberId = subscriber.Id,
                Name = subscriber.Name,
                Msisdn = subscriber.Msisdn,
                SubscriptionDate = DateTime.Now,
                MoMoBalance = accountBalance.AvailableBalance,
                OutstandingBalance = subscriber.Loans?.Where(l => !l.IsSettled)?.Sum(l => l.Amount) ?? 0,
                ActiveClients = subscriber.Loans?
                .Where(l => !l.IsSettled)?
                .Select(l => new ActiveClientsDto
                {
                    Id = l.SubscriberClient.Id,
                    Name = l.SubscriberClient.Name,
                    Balance = l.Amount,
                    Msisdn = l.SubscriberClient.Msisdn
                })?.ToList()
            };
        }
    }
}
